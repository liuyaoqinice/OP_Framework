using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

//****************************
//创建人: liu
//功能说明： 
//****************************

public enum SVN_TYPE
{
    UPDATE,
    REVERT,
    DELETE,
    COMMIT,
    CLEAN_UP,
    GET_VERSION,
}

public class SVNUpdate : Editor
{
    private const string SVN_COMMIT = "Assets/SVN/Commit";
    private const string SVN_UPDATE = "Assets/SVN/Update";

    private const string COMMIT = "commit";
    private const string UPDATE = "update";

    private static readonly string _assetsPath = Application.dataPath;
    private static string _rootPath = _assetsPath.Substring(0, _assetsPath.Length - 6);
    private static string _revertPath = (_assetsPath + "/../ITools/SVNMgr").Replace('/', '\\');
    private static string _configPath = (_assetsPath + "/../Config").Replace('/', '\\');

	[MenuItem("Tools/SVN工具/SVN Update/SVN UpdateAll %#&Q")]
	private static void SvnUpdateAllClient()
	{
		var path = GetDirPath();
		SVNCommand(path, "update", true);
	}

	[MenuItem("Tools/SVN工具/还原工具/本地与svn保持一致")]
	public static void RevertClient()
	{
		var process = new Process();
		process.StartInfo.WorkingDirectory = _revertPath;
		process.StartInfo.FileName = "与svn保持一致.bat";
		process.Start();
	}

	/// <summary>
	/// 打资源svn工具参数说明list(文件路径),num(0.跟新，1.还原，2.删除，3.上传，4.CleanUp，5.获取版本号)
	/// </summary>
	/// <param name="_list"></param>
	/// <param name="num"></param>
	public static void Ctrl(SVN_TYPE num, params string[] _list)
	{
		var _itemPath = _assetsPath + "/../ITools/SVNMgr/SVNMgr/SVNMgr/bin/Debug/filePath.txt";
		if (!File.Exists(_itemPath))
		{
			File.CreateText(_itemPath).Dispose();
		}
		else
		{
			File.Delete(_itemPath);
			File.CreateText(_itemPath).Dispose();
		}

		var fs = new FileStream(_itemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		var sw = new StreamWriter(fs);
		foreach (var filePath in _list)
		{
			var path = filePath + " " + ((int)num).ToString();
			sw.WriteLine(path);
		}
		sw.Close();

		var svnPath = (_assetsPath + "/../ITools/SVNMgr/SVNMgr/SVNMgr/bin/Debug").Replace('/', '\\');
		var process = new Process();
		process.StartInfo.WorkingDirectory = svnPath;
		process.StartInfo.FileName = "SVNMgr.exe";
		process.Start();
		process.WaitForExit();
	}

	[MenuItem("Tools/SVN工具/SVN Update/SVN UpdateClient")]
	private static void UpdateWork()
	{
		SVNCommand(_assetsPath, "update");
	}
	[MenuItem("Tools/SVN工具/SVN Update/SVN UpdateLua")]
	private static void SvnUpdateLua()
	{
		var LuaPath = _assetsPath + "/lua*" + _assetsPath + "/Lua.meta*";
		SVNCommand(LuaPath, "update", true);
	}

	[MenuItem("Tools/SVN工具/SVN Update/SVN UpdateArtModel")]
	public static void SvnUpdateArtModel()
	{
		var LuaPath = _assetsPath + "/Art/Model*" + _assetsPath + "/Art/Model.meta*";
		SVNCommand(LuaPath, "update", true);
	}

	[MenuItem("Tools/SVN工具/SVN Update/SVN UpdateResModel")]
	public static void SvnUpdateResModel()
	{
		var LuaPath = _assetsPath + "/Res/Model*" + _assetsPath + "/Res/Model.meta*";
		SVNCommand(LuaPath, "update", true);
	}

	[MenuItem("Tools/SVN工具/SVN Commit/Client Commit %#&W")]
	private static void SvnCommit()
	{
		var _Path = _assetsPath + "*" + _configPath + "*" ; 
		SVNCommand(_Path, "commit",true);
	}

	private static string GetDirPath()
	{
		//string protoBufPath = rootPath + "ITools/BuilderProtobuf" + "*";
		var luaPath = _assetsPath + "/Lua";
		var DirPath = _rootPath + "*" + luaPath + "*" + luaPath + ".meta*" ;//+ protoBufPath
		return DirPath;
	}

	/// <summary>  
	/// 获取全部选中物体的路径  
	/// 包括meta文件  
	/// </summary>  
	/// <returns></returns>  
	private static string GetSelectedObjectPath()
	{
		var path = string.Empty;
		
		foreach (var t in Selection.objects)
		{
			path += AssetsPathToFilePath(AssetDatabase.GetAssetPath(t));
			//路径分隔符  

			path += "*";
			//meta文件  
			path += AssetsPathToFilePath(AssetDatabase.GetAssetPath(t)) + ".meta";
			//路径分隔符  
			path += "*";
		}
		// if(DebugMgr.CanLogError()) DebugMgr.LogError(path);
		//UnityEngine.Debug.Log(path);
		return path;
	}
	/// <summary>  
	/// 将Assets路径转换为File路径  
	/// </summary>  
	/// <param name="path">Assets/Editor/...</param>  
	/// <returns></returns>   
	private static string AssetsPathToFilePath(string path)
	{

		var m_path = Application.dataPath;
		m_path = m_path.Substring(0, m_path.Length - 6);

		m_path += path;
		return m_path;
	}

	/// <summary>  
	/// 跟新选中 
	/// </summary>  
	[MenuItem(SVN_UPDATE)]
	private static void SVNUpdateSelection()
	{
		SVNCommand(GetSelectedObjectPath(), UPDATE, true);
	}

	/// <summary>  
	/// 提交选中内容  
	/// </summary>  
	[MenuItem(SVN_COMMIT)]
	public static void SVNCommitSelection()
	{
		SVNCommand(GetSelectedObjectPath(), COMMIT, true);
	}

	private static void SVNCommand(string path, string command, bool isSelction = false)
	{
		if (Directory.Exists(path) || isSelction)
		{
			var process = new Process();
			process.StartInfo.FileName = "TortoiseProc.exe";
			process.StartInfo.Arguments = $"/command:{command} /path:{path}";
			process.Start();
		}
		else
		{
			UnityEngine.Debug.Log("文件路径不存在！！！" + path);
		}
	}
}
