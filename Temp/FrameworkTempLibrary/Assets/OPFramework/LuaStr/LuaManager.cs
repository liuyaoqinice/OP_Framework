using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text;
using XLua;

namespace OPFramework
{
    public static class LuaManager
    {
        static LuaManager()
        {
            Init();
        }

        public static LuaEnv lua { get; private set; }

        public static void Init()
        {
            if (lua != null) return;
            lua = new LuaEnv();

            
            lua.AddBuildin("pb", XLua.LuaDLL.Lua.LoadProtobuf);
            lua.AddBuildin("pb_unsafe", XLua.LuaDLL.Lua.LoadProtobufUnsafe);
        }

        public static void Dispose()
        {
            lua.Dispose();
            lua = null;
        }

        public static void Release()
        {
            lua = null;
        }

        private static string GetFullPath(string path)
        {
            return path;
        }

        private const int BUFFER_SIZE = 1024 * 1024;
        private static byte[] _buffer = new byte[BUFFER_SIZE];
        private static byte[] _largeBuffer;

        private static byte[] GetLargeBuffer(int length)
        {
            if (_largeBuffer == null || length > _largeBuffer.Length)
            {
                _largeBuffer = new byte[length + BUFFER_SIZE];
            }

            return _largeBuffer;
        }

        private static byte[] GetBuffer(int length)
        {
            return length > _buffer.Length ? GetLargeBuffer(length) : _buffer;
        }

        // 读取文件流
        public static Stream OpenFile(string path)
        {
#if UNITY_EDITOR
            return File.OpenRead(path);
#else
            
#endif
        }

        public static byte[] LoadFile(string path, out int length)
        {
            var stream = OpenFile(path);
            if (stream == null)
            {
                length = 0;
                return null;
            }

            length = (int)stream.Length;
            var buffer = GetBuffer(length);
            //stream.Read(buffer); TODO
            stream.Dispose();
            return buffer;
        }

        public static byte[] LoadFile(string path)
        {
            var stream = OpenFile(path);
            if (stream == null)
            {
                return null;
            }

            var buffer = stream.ReadByte();
            stream.Dispose();
            return null; // TODO
        }

        public static string LoadFileText(string path , Encoding encoding = null)
        {
            var stream = OpenFile(path);
            if (stream == null)
            {
                return null;
            }

            //var text = stream.ReadAllText(); TODO
            stream.Dispose();
            return "";
        }

        public static bool Exists(string path)
        {
            // TODO 资源加载管理器
            return false;
        }


        public static void Tick()
        {
            if (_largeBuffer != null)
                _largeBuffer = null;
            
            if(lua != null)
                lua.Tick();
        }


        public static void Require(string filePath , bool reload = false)
        {
            if (reload)
            {
                lua.DoString($"package.loaded['{filePath}'] = nil");
            }

            lua.DoString($"require '{filePath}'");
        }

        private static readonly List<Action> releaseActions = new List<Action>();

        public static void ReleaseAllGlobal()
        {
            foreach (var action in releaseActions)
            {
                action();
            }
            releaseActions.Clear();
        }

        public static void GetGlobal<T>(string path,out T value ,Action releaseAction = null)
        {
            var names = path.Split(".:".ToCharArray());
            var t = lua.Global;
            for (int i = 0; i < names.Length -1; ++i)
            {
                t.Get(names[i], out t);
                if (t == null)
                {
                    value = default(T);
                    return;
                }
            }

            t.Get(names[names.Length - 1], out value);

            if (releaseAction != null)
                releaseActions.Add(releaseAction);
        }

        public static T GetGlobal<T>(string path , Action releaseAction = null)
        {
            GetGlobal(path,out T value , releaseAction);
            return value;
        }
        
    }
}