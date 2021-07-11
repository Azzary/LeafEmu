using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LeafEmu.World.Network.AsyncSocket
{
    /// <summary>
    /// Associated with each customer Socket, the parameters required for Send and Receive delivery
    /// </summary>
    internal sealed class IoContextPool
    {
        List<SocketAsyncEventArgs> pool;        //Each Socket client is assigned a SocketAsyncEventArgs, managed by a List, which is set up at program startup.
        Int32 capacity;                         //Capacity of pool object pool
        Int32 boundary;                         //The boundaries of the allocated and unallocated objects are large ones that have been allocated and small ones that have not been allocated.

        internal IoContextPool(Int32 capacity)
        {
            this.pool = new List<SocketAsyncEventArgs>(capacity);
            this.boundary = 0;
            this.capacity = capacity;
        }

        /// <summary>
        /// Add newly created objects to the pool object pool, because the program builds all objects at startup.
        /// So this method is only called when initialized, so there is no lock.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        internal bool Add(SocketAsyncEventArgs arg)
        {
            if (arg != null && pool.Count < capacity)
            {
                pool.Add(arg);
                boundary++;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Remove the specified object from the collection for internal use
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //internal SocketAsyncEventArgs Get(int index)
        //{
        //    if (index >= 0 && index < capacity)
        //        return pool[index];
        //    else
        //        return null;
        //}

        /// <summary>
        /// Remove an object from the object pool and hand it to a socket for delivery of requests
        /// </summary>
        /// <returns></returns>
        internal SocketAsyncEventArgs Pop()
        {
            lock (this.pool)
            {
                if (boundary > 0)
                {
                    --boundary;
                    return pool[boundary];
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// A socket customer disconnects, and its associated IoContext is released and re-invested in Pool for standby.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        internal bool Push(SocketAsyncEventArgs arg)
        {
            if (arg != null)
            {
                lock (this.pool)
                {
                    int index = this.pool.IndexOf(arg, boundary);  //Find out the disconnected customers, you can find them here, so index can not be - 1, it must be greater than 0.
                    if (index == boundary)         //It's just the boundary element.
                        boundary++;
                    else
                    {
                        this.pool[index] = this.pool[boundary];     //Move the disconnected customer to the boundary and the boundary to the right
                        this.pool[boundary++] = arg;
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}
