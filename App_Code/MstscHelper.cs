﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for MstscHelper
/// </summary>
public class MstscHelper : RemoteClientHelper
{
	private const string tableName = "mstsc";
	private const string initialLink = "wbr://type=1&progname=mstsc.exe";
	private const int number = 1;
	private const string columnIp = "ip";
	private const string columnPort = "port";
	private const string columnUsername = "username";
	private const string columnPassword = "password";

	//private string ip = "";
	public string Ip
	{
		get { return Parameters[columnIp]; }
		set { Parameters[columnIp] = value; }
	}
	//private string port = "";
	public string Port
	{
		get { return Parameters[columnPort]; }
		set { Parameters[columnPort] = value; }
	}
	//private string username = "";
	public string Username
	{
		get { return Parameters[columnUsername]; }
		set { Parameters[columnUsername] = value; }
	}
	//private string password = "";
	public string Password
	{
		get { return Parameters[columnPassword]; }
		set { Parameters[columnPassword] = value; }
	}
	private Dictionary<string, string> parameters = new Dictionary<string, string>()
	{
	        {columnIp, ""},
	        {columnPort, ""},
	        {columnUsername, ""},
	        {columnPassword, ""},
	};
	public override Dictionary<string, string> Parameters
	{
		get { return parameters; }
	}

	public static RemoteClientHelper GetRemoteClientHelper(string id, string serverType)
	{
		MstscHelper helper = new MstscHelper();
		string sql = "select ";
		foreach (string field in helper.Parameters.Keys)
		{
			sql += field;
			sql += ",";
		}
		sql = sql.Substring(0, sql.Length - 1);
		sql += " from " + MstscHelper.tableName;
		sql += " where id=" + id;
		sql += " and server_type='" + serverType + "'";
		DataSet ds = DBAccess.GetDataSet(sql, MstscHelper.tableName);
		if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
		{
			DataTable dt = ds.Tables[MstscHelper.tableName];
			foreach (string field in new List<string>(helper.Parameters.Keys))
			{
				//kvp.Value = dt.Rows[0][kvp.Key].ToString();
				helper.Parameters[field] = dt.Rows[0][field].ToString();
			}
		}
		return helper;
	}
	private MstscHelper() { }

	public override string CreateLink(string defaultValue)
	{
		string link = defaultValue;
		if (Ip.Length > 0)
		{
			link = initialLink;
			Catenate(ref link, columnIp, Ip);
			if (Port.Length > 0)
				Catenate(ref link, columnPort, Port);
			if (Username.Length > 0)
				Catenate(ref link, columnUsername, Username);
			if (Password.Length > 0)
				Catenate(ref link, columnPassword, Password);
		}
		return link;
	}
}
