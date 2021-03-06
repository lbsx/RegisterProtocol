﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using RemoteControl.Data.Base;
using RemoteControl.Data.BusinessObjects;
using RemoteControl.Data.ManagerObjects;
/// <summary>
/// Summary description for RadminHelper
/// </summary>
public class RadminHelper : RemoteClientHelper
{
	private const string tableName = "radmin";
	private const string initialLink = "wbr://type=0&progname=radmin.exe";
	private const int number = 0;
	public const string columnIp = "ip";
	public const string columnPort = "port";
	public const string columnUsername = "username";
	public const string columnPassword = "password";

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
		RadminHelper helper = new RadminHelper();
		string sql = "select ";
		foreach (string field in helper.Parameters.Keys)
		{
			sql += field;
			sql += ",";
		}
		sql = sql.Substring(0, sql.Length - 1);
		sql += " from " + RadminHelper.tableName;
		sql += " where ci_id=" + id;
		sql += " and server_type='" + serverType + "'";
		DataSet ds = DBAccess.GetDataSet(sql, RadminHelper.tableName);
		if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
		{
			DataTable dt = ds.Tables[RadminHelper.tableName];
			foreach (string field in new List<string>(helper.Parameters.Keys))
			{
				//kvp.Value = dt.Rows[0][kvp.Key].ToString();
				helper.Parameters[field] = dt.Rows[0][field].ToString();
			}
		}
		return helper;
	}
	private RadminHelper() { }

	public static string CreateLink(Radmin radmin, string defaultValue)
	{
		if (radmin == null)
			return defaultValue;
		string link = defaultValue;
		if (radmin.rIp != null && radmin.rIp.Length > 0)
		{
			link = initialLink;
			RemoteClientHelper.Catenate(ref link, columnIp, radmin.rIp);
			if (radmin.rPort != null && radmin.rPort.Length > 0)
				Catenate(ref link, columnPort, radmin.rPort);
			if (radmin.rUsername != null && radmin.rUsername.Length > 0)
				Catenate(ref link, columnUsername, radmin.rUsername);
			if (radmin.rPassword != null && radmin.rPassword.Length > 0)
				Catenate(ref link, columnPassword, radmin.rPassword);
		}
		return link;
	}
}
