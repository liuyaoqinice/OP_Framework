﻿namespace XLua.LuaDLL
{
    using System.Runtime.InteropServices;
    public partial class Lua
    {
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadProtobuf(System.IntPtr L)
        {
            return luaopen_pb(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb_unsafe(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadProtobufUnsafe(System.IntPtr L)
        {
            return luaopen_pb_unsafe(L);
        }
    }
}