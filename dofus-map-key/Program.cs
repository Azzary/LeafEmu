using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Web;

/*

    Smarken 2017

    Thanks to Manghao for making me mad

 */
namespace DofusKeyFinder
{
    public class Map
    {
        public int Id;
        public int Width;
        public int Height;
        public string Key;
        public string DecodedData;
        public string EncodedData;
    }

    public class Statistics
    {
        public double IndexOfCoincidence;
        public Dictionary<char, double> GlobalFrequencies;
        public Dictionary<char, double>[] PositionFrequencies;
    }

    public class ComputedKey
    {
        public string Value;
        public Dictionary<int, List<(int, double)>> Alternatives;
    }

    /*
        Achieved 90% success on finding a key
     */
    class Program
    {
        const int ValidKeyAltThreshold = 15;
        const double ErrorClamp = 1.15;
        const int CellPatternSize = 10;
        const int MinKeyLength = 128;
        const int MaxKeyLength = 256;
        const int MinKeyXor = 32;
        const int MaxKeyXor = 127;
        public static string HEX_CHARS = "0123456789ABCDEF";
        public static string MapsPath = "../maps";
        public static string StatisticsPath = "./statistics.json";
        public static string InputPath = "./input";
        public static string OutputPath = "./output";

        public static IEnumerable<Map> LoadMaps()
        {
            return Directory.GetFiles(MapsPath, "*.json")
                .Select(file => JsonConvert.DeserializeAnonymousType<Map>(File.ReadAllText(file), new Map()));
        }

        public static Statistics LoadStatistics()
        {
            return JsonConvert.DeserializeAnonymousType<Statistics>(File.ReadAllText(StatisticsPath), new Statistics());
        }

        public static void SaveStatistics(Statistics statistics)
        {
            File.WriteAllText(StatisticsPath, JsonConvert.SerializeObject(statistics, Formatting.Indented));
        }

        public static void Main(String[] args)
        {
            var statistics = LoadStatistics();
            //GenerateStatistics();
            //Benchmark(statistics);
            ExportCompute(statistics);
        }

        public static void GenerateStatistics()
        {
            // INITIALIZE
            var statistics = new Statistics()
            {
                GlobalFrequencies = new Dictionary<char, double>(),
                PositionFrequencies = new Dictionary<char, double>[CellPatternSize]
            };
            for (var i = 0; i < CellPatternSize; i++)
            {
                statistics.PositionFrequencies[i] = new Dictionary<char, double>();
            }

            // Load
            var maps = LoadMaps();

            // Frequencies
            var totalDecodedLength = 0;
            var totalCoincidence = 0.0;
            foreach (var map in maps)
            {
                totalCoincidence += ComputeCoincidenceRate(map.DecodedData);
                totalDecodedLength += map.DecodedData.Length;
                for (var i = 0; i < map.DecodedData.Length; i++)
                {
                    var currentLetter = map.DecodedData[i];
                    if (!statistics.GlobalFrequencies.ContainsKey(currentLetter))
                    {
                        statistics.GlobalFrequencies[currentLetter] = 1;
                    }
                    else
                    {
                        statistics.GlobalFrequencies[currentLetter]++;
                    }
                    var patternIndex = i % CellPatternSize;
                    var positionFrequencies = statistics.PositionFrequencies[patternIndex];
                    if (!positionFrequencies.ContainsKey(currentLetter))
                    {
                        positionFrequencies[currentLetter] = 1;
                    }
                    else
                    {
                        positionFrequencies[currentLetter]++;
                    }
                }
            }

            statistics.IndexOfCoincidence = totalCoincidence / maps.Count();

            // Normalize
            foreach (var k in statistics.GlobalFrequencies.Keys.ToList())
            {
                statistics.GlobalFrequencies[k] /= totalDecodedLength;
            }
            for (var i = 0; i < CellPatternSize; i++)
            {
                var positionFrequencies = statistics.PositionFrequencies[i];
                foreach (var k in positionFrequencies.Keys.ToList())
                {
                    positionFrequencies[k] /= totalDecodedLength / CellPatternSize;
                }
            }

            SaveStatistics(statistics);

            // Check
            //var globalTotalFreq = statistics.GlobalFrequencies.Values.Aggregate(0.0, (acc, freq) => acc + freq);
            //var positionTotalFreq = statistics.PositionFrequencies.Sum(p => p.Values.Aggregate(0.0, (acc, freq) => acc + freq));
        }

        public static void Benchmark(Statistics statistics)
        {
            var hit = 0;
            var tih = 0;
            var bad = 0;
            var maps = LoadMaps();
            foreach (var map in LoadMaps())
            {
                tih++;
                var realKey = PrepareKey(map.Key);
                var encodedData = HexToString(map.EncodedData);
                var keyLengthHamming = ComputeKeyLengthHamming(encodedData);

                // FAR BETTER
                var keyLengthFriedman = ComputeKeyLengthFriedman(statistics, encodedData);
                //var keyLengthKasiski = ComputeKeyLengthKasiski(encodedData);
                //var keyLengthFreq = ComputeKeyLengthFrequency(encodedData);

                if (keyLengthFriedman != realKey.Length)
                {
                    Logger.Log($"r={realKey.Length}, f={keyLengthFriedman}, h={keyLengthHamming}");
                }
                var key = GuessKey(encodedData, keyLengthFriedman, statistics);
                if (key.Value == realKey)
                {
                    if (key.Alternatives.Count > 0)
                    {
                        Logger.Log($"ALT: {map.Id}, {key.Alternatives.Count}");
                    }
                    hit++;
                }
                else
                {
                    if (key.Alternatives.Count > 0)
                    {
                        bad++;
                        Logger.Log(key.Alternatives.Count);
                    }
                    else
                    {
                        Logger.Log($"IMP: {map.Id}, {key.Alternatives.Count}");
                    }
                }
                Logger.Log(hit / (double)tih);
            }
        }


        public static void Turn(int i, int max)
        {
            Console.Write(i + "/" + max);
            Console.SetCursorPosition(Console.CursorLeft - (i.ToString() + "/" + max).Length, Console.CursorTop);
        }

        public static void ExportCompute(Statistics statistics)
        {
            string connStr = $"server = localhost; user = root; database = leafworld602;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            mapdb.loadMap(conn);

            int len = Directory.GetFiles(InputPath, "*").Length;
            int i = 0;
            Directory.CreateDirectory(InputPath);
            Directory.CreateDirectory(OutputPath);
            Directory.GetFiles(InputPath, "*")
                .ToObservable()
                .Select(path => new { EncodedData = HexToString(File.ReadAllText(path)), Name = Path.GetFileName(path) })
                .Do(x => i++)
                .Do(x => Turn(i, len))
                .Select(x => new { Header = x, KeyLength = ComputeKeyLengthFriedman(statistics, x.EncodedData) })
                .Select(y => new { Data = y, Key = GuessKey(y.Header.EncodedData, y.KeyLength, statistics) })
                .Do(z => mapdb.changeKey(Convert.ToInt32(z.Data.Header.Name.Split('_')[0]), z.Data.Header.Name.Split('_')[1].Split('.')[0],
                FormatKeyExport(z.Key.Value)))//File.WriteAllText(string.Format("{0}/{1}", OutputPath, z.Data.Header.Name + "_key.txt"), FormatKeyExport(z.Key.Value)))
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(z =>
                {

                });
        }

        public static string Checksum(string key)
        {
            int checksum = 0;
            for (int i = 0; i < key.Length; i++)
            {
                checksum += char.ConvertToUtf32(key, i) % 16;
            }
            return char.ToString(HEX_CHARS[checksum % 16]);
        }

        public static string Decrypt(string data, string key)
        {
            int shift = int.Parse(Checksum(key), NumberStyles.HexNumber) * 2;
            var decrypted = new StringBuilder(data.Length);
            int keyLength = key.Length;
            for (var i = 0; i < data.Length; i++)
            {
                var currentData = data[i];
                var currentKey = key[(i + shift) % keyLength];
                decrypted.Append((char)(currentData ^ currentKey));
            }
            return HttpUtility.UrlDecode(decrypted.ToString());
        }

        public static string HexToString(string data)
        {
            var output = new StringBuilder(data.Length / 2);
            for (var i = 0; i < data.Length; i += 2)
            {
                output.Append((char)int.Parse(data.Substring(i, 2), NumberStyles.HexNumber));
            }
            return output.ToString();
        }

        public static string PrepareKey(string data)
        {
            return HttpUtility.UrlDecode(HexToString(data));
        }

        public static string PreEscape(string input)
        {
            var output = new StringBuilder();
            foreach (var c in input)
            {
                if (c < 32 || c > 127 || c == '%' || c == '+')
                {
                    output.Append(HttpUtility.UrlEncode(char.ToString(c)));
                }
                else
                {
                    output.Append(c);
                }
            }
            return output.ToString();
        }


        public static HashSet<int> ComputeMaxShifts(string s)
        {
            var shifts = new HashSet<int>();
            var matchPos = 0;
            var minLength = 3;
            for (int shift = 1; shift < s.Length; shift++)
            {
                int matchCount = 0;
                for (int i = 0; i < s.Length - shift; i++)
                {
                    if (s[i] == s[i + shift])
                    {
                        matchCount++;
                        if (matchCount >= minLength)
                        {
                            matchPos = i - matchCount + 1;
                            shifts.Add(shift);
                        }
                    }
                    else
                    {
                        matchCount = 0;
                    }
                }
            }
            return shifts;
        }

        public static string LeftRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(shift) + key.Substring(0, shift);
        }

        public static string RightRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(key.Length - shift) + key.Substring(0, key.Length - shift);
        }

        private static int GCD(int a, int b)
        {
            if (a >= MinKeyLength && a <= MaxKeyLength)
                return a;
            else if (b >= MinKeyLength && b <= MaxKeyLength)
                return b;
            else if (a == 0 || b == 0)
                return a | b;
            return GCD(Math.Min(a, b), Math.Max(a, b) % Math.Min(a, b));
        }

        public static int ComputeKeyLengthKasiski(string encodedData)
        {
            return ComputeMaxShifts(encodedData)
                .Where(shift => shift >= MinKeyLength)
                .Aggregate(GCD);
        }

        public static double ComputeCoincidenceRate(string encodedData)
        {
            return ComputeCoincidenceRate(encodedData, 0, encodedData.Length);
        }

        public static double ComputeCoincidenceRate(string encodedData, int offset, int count)
        {
            return ComputeCoincidenceRate(GetFrequencies(encodedData, offset, count), count);
        }

        public static double ComputeCoincidenceRate(Dictionary<char, double> frequencies, int length)
        {
            return frequencies
                .Values
                .Select(value => value * length)
                .Aggregate(0.0, (acc, freq) => acc + ((freq * (freq - 1)) / (length * (length - 1))));
        }

        public static int ComputeKeyLengthFrequency(string encodedData)
        {
            var bestKeyLength = -1;
            var bestDistance = double.MaxValue;
            for (var keyLength = MinKeyLength; keyLength <= MaxKeyLength; keyLength++)
            {
                var numberOfBlock = encodedData.Length / keyLength;
                var currentDistance = 0.0;
                for (var j = 0; j < numberOfBlock - 1; j++)
                {
                    var x = j * keyLength;
                    var xFreq = GetFrequencies(encodedData, x, keyLength);
                    for (var k = j + 1; k < numberOfBlock; k++)
                    {
                        var y = k * keyLength;
                        var yFreq = GetFrequencies(encodedData, y, keyLength);
                        currentDistance = GetDistance(xFreq, yFreq);
                    }
                }
                currentDistance = currentDistance / encodedData.Length;
                if (currentDistance < bestDistance)
                {
                    bestKeyLength = keyLength;
                    bestDistance = currentDistance;
                }
            }
            return bestKeyLength;
        }

        public static int ComputeKeyLengthFriedman(Statistics statistics, string encodedData)
        {
            var bestCoincidence = double.MinValue;
            var bestKeyLength = -1;
            for (var keyLength = MinKeyLength; keyLength <= MaxKeyLength; keyLength++)
            {
                var totalCoincidence = 0.0;
                var numberOfBlock = (int)Math.Ceiling(encodedData.Length / (double)keyLength);
                var offsetOverflow = encodedData.Length % keyLength;
                for (var blockOffset = 0; blockOffset < keyLength; blockOffset++)
                {
                    var numberOfBlockAffected = offsetOverflow > 0 ?
                          blockOffset > offsetOverflow ? numberOfBlock - 1 : numberOfBlock : numberOfBlock;
                    var cryptedBlock = new char[numberOfBlockAffected];
                    for (var blockNumber = 0; blockNumber < numberOfBlock; blockNumber++)
                    {
                        var absoluteOffset = blockNumber * keyLength + blockOffset;
                        if (absoluteOffset < encodedData.Length)
                        {
                            cryptedBlock[blockNumber] = encodedData[absoluteOffset];
                        }
                    }
                    totalCoincidence += ComputeCoincidenceRate(new string(cryptedBlock));
                }
                var coincidence = totalCoincidence / keyLength;
                if (coincidence >= bestCoincidence)
                {
                    bestCoincidence = coincidence;
                    bestKeyLength = keyLength;
                }
            }
            return bestKeyLength;
        }

        public static int ComputeKeyLengthHamming(string encodedData)
        {
            var bestKeyLength = 0;
            var bestScore = int.MaxValue;
            for (var keyLength = MinKeyLength; keyLength <= MaxKeyLength; keyLength++)
            {
                var numberOfBlock = encodedData.Length / keyLength;
                var currentScore = 0;
                for (var j = 0; j < numberOfBlock - 1; j++)
                {
                    var x = j * keyLength;
                    for (var k = j + 1; k < numberOfBlock; k++)
                    {
                        var y = k * keyLength;
                        currentScore += ComputeHammingDistance(encodedData, x, y, keyLength);
                    }
                }
                currentScore = currentScore / encodedData.Length;
                if (currentScore < bestScore)
                {
                    bestKeyLength = keyLength;
                    bestScore = currentScore;
                }
            }
            return bestKeyLength;
        }

        public static int ComputeHammingDistance(string message, int x, int y, int keyLength)
        {
            var distance = 0;
            for (var i = 0; i < keyLength; i++)
            {
                var cA = message[x + i];
                var cB = message[y + i];
                var xor = cA ^ cB;
                while (xor != 0)
                {
                    distance++;
                    xor &= xor - 1;
                }
            }
            return distance;
        }

        private static ComputedKey GuessKey(string message, int keyLength, Statistics statistics)
        {
            var alternatives = new Dictionary<int, List<(int, double)>>();
            var blockSize = keyLength;
            var numberOfBlock = (int)Math.Ceiling(message.Length / (double)blockSize);
            var offsetOverflow = message.Length % blockSize;
            var key = new StringBuilder();
            for (var blockOffset = 0; blockOffset < blockSize; blockOffset++)
            {
                alternatives.Add(blockOffset, new List<(int, double)>());
                var bestError = double.MaxValue;
                var bestXor = -1;
                // Only between thoses
                for (var xorKey = MinKeyXor; xorKey <= MaxKeyXor; xorKey++)
                {
                    var numberOfBlockAffected = offsetOverflow > 0 ?
                        blockOffset > offsetOverflow ? numberOfBlock - 1 : numberOfBlock : numberOfBlock;
                    var decryptedBlock = new char[numberOfBlockAffected];
                    for (var blockNumber = 0; blockNumber < numberOfBlock; blockNumber++)
                    {
                        var absoluteOffset = blockNumber * blockSize + blockOffset;
                        if (absoluteOffset < message.Length)
                        {
                            var currentData = message[absoluteOffset];
                            decryptedBlock[blockNumber] = (char)(currentData ^ xorKey);
                        }
                    }
                    var decrypted = HttpUtility.UrlDecode(new string(decryptedBlock));
                    var error = ComputeError(decrypted, blockOffset, blockSize, statistics);
                    if (error <= bestError)
                    {
                        if (error == bestError)
                        {
                            if (bestXor != -1)
                            {
                                alternatives[blockOffset].Add((xorKey, error));
                            }
                        }
                        else
                        {
                            var clampedValue = error * ErrorClamp;
                            alternatives[blockOffset].RemoveAll(x => x.Item2 > clampedValue);
                            alternatives[blockOffset].Add((xorKey, error));
                        }
                        bestError = error;
                        bestXor = xorKey;
                    }
                }
                key.Append((char)bestXor);
            }
            // Shift back the key with its checksum
            var computedKey = key.ToString();
            int shift = int.Parse(Checksum(computedKey), NumberStyles.HexNumber) * 2;
            var finalKey = RightRotateShift(computedKey, shift);
            return new ComputedKey
            {
                Value = finalKey,
                Alternatives = alternatives.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x.Value)
            };
        }

        private static double ComputeError(string decrypted, int blockOffset, int blockSize, Statistics statistics)
        {
            var frequencies = GetFrequencies(decrypted);
            return GetPositionError(decrypted, blockOffset, blockSize, statistics);
        }

        public static double GetPositionError(string decrypted, int blockOffset, int blockSize, Statistics statistics)
        {
            var distance = 0.0;
            for (var i = 0; i < decrypted.Length; i++)
            {
                var absolutePosition = blockSize * i + blockOffset;
                var positionStatistics = statistics.PositionFrequencies[absolutePosition % CellPatternSize];
                var currentData = decrypted[i];
                if (positionStatistics.ContainsKey(currentData))
                {
                    distance += (1 - positionStatistics[currentData]);
                }
                else
                {
                    distance += 10;
                }
            }
            return distance;
        }

        // Squared Euclidean distance
        public static double GetDistance(Dictionary<char, double> u, Dictionary<char, double> v)
        {
            var distance = 0.0;
            foreach (var kv in u)
            {
                if (v.ContainsKey(kv.Key))
                {
                    distance += Math.Abs(kv.Value - v[kv.Key]);
                }
                else
                {
                    distance += kv.Value;
                }
            }
            foreach (var kv in v)
            {
                if (!u.ContainsKey(kv.Key))
                {
                    distance += kv.Value;
                }
            }
            return distance;
        }

        private static Dictionary<char, double> GetFrequencies(string input)
        {
            return GetFrequencies(input, 0, input.Length);
        }

        private static Dictionary<char, double> GetFrequencies(string input, int offset, int count)
        {
            var statistics = new Dictionary<char, double>();
            for (var i = offset; i < offset + count; i++)
            {
                var c = input[i];
                if (statistics.ContainsKey(c))
                    statistics[c]++;
                else
                    statistics[c] = 1;
            }
            return statistics.ToDictionary(kv => kv.Key, kv => (kv.Value / count));
        }

        public static string FormatKeyExport(string key)
        {
            return PreEscape(key).Select(c => String.Format("{0:X}", (int)c)).Aggregate("", (acc, c) => acc + c);
        }
    }
}
