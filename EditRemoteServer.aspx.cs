﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RemoteControl.Data.Base;
using RemoteControl.Data.BusinessObjects;
using RemoteControl.Data.ManagerObjects;

public partial class EditRemoteServer : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string id = Request["id"];
		if (id != null && id.Length > 0)
		{
			if (!this.IsPostBack)
			{
				CafeInformation ci = GetCafeInformation(id);
				FillCafeInformationControls(ci);
				SetRadioButtonSelected(ci);
				BindServersValue(ci);
			}
		}
		DisplayPageControls(true);
	}

	private void BindServersValue(CafeInformation ci)
	{
		List<string> serverTypes = new List<string>()
	        {
	                "main_server", "secondary_server", "cash_register_server", "movie_server", "router_server", 
	        };

		foreach (string serverType in serverTypes)
		{
			BindServerValue(ci, serverType);
		}
	}

	/// <summary>
	/// 绑定具体某个ID下的其中一个服务器的配置信息
	/// 在PostBack的情况下，如果用户选择的RadioButton与服务器中的不一样
	/// 说明用户可能要改变该服务器的配置
	/// </summary>
	/// <param name="id"></param>
	/// <param name="serverType"></param>
	private void BindServerValue(CafeInformation ci, string serverType)
	{
		if (ci == null)
			return;
		string id = ci.Id;
		string serverColumnName = NameHelper.GetServerTypeColumnName(serverType);

		string remoteClientType = GetRemoteClientName(ci, serverType);
		CheckBox serverEnable = (CheckBox)FindControl("CheckBoxEnable" + NameHelper.GetControlIdInfix(serverType));
		if (remoteClientType == null || remoteClientType.Length == 0)
		{
			serverEnable.Checked = false;
			return;
		}
		serverEnable.Checked = true;
		RadioButtonList rbl = GetRadioButtonList(serverType); 
		if (rbl.SelectedValue == remoteClientType)
		{
			string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
			List<string> controlIdSuffices = NameHelper.GetControlIdSuffices(remoteClientType);
			switch (remoteClientType)
			{
				case "radmin":
					IRadminManager radminManager = GetRadminManager(true);
					Radmin radmin = radminManager.GetById(id, serverType);
					BindRadminValue(radmin, controlIdInfix);
					radminManager.Dispose();
					break;
				case "mstsc":
					IMstscManager mstscManager = GetMstscManager(true);
					Mstsc mstsc = mstscManager.GetById(id, serverType);
					BindMstscValue(mstsc, controlIdInfix);
					mstscManager.Dispose();
					break;
				case "ttvnc":
					ITtvncManager ttvncManager = GetTtvncManager(true);
					Ttvnc ttvnc = ttvncManager.GetById(id, serverType);
					BindTtvncValue(ttvnc, controlIdInfix);
					ttvncManager.Dispose();
					break;
				case "teamviewer":
					ITeamviewerManager teamviewerManager = GetTeamviewerManager(true);
					Teamviewer teamviewer = teamviewerManager.GetById(id, serverType);
					BindTeamviewerValue(teamviewer, controlIdInfix);
					teamviewerManager.Dispose();
					break;
				case "remotelyanywhere":
					IRemotelyanywhereManager remotelyanywhereManager = GetRemotelyanywhereManager(true);
					Remotelyanywhere remotelyanywhere = remotelyanywhereManager.GetById(id, serverType);
					BindRemotelyanywhereValue(remotelyanywhere, controlIdInfix);
					remotelyanywhereManager.Dispose();
					break;
			}
		}
	}

	private void BindRadminValue(Radmin radmin, string controlIdInfix)
	{
		string controlIdPrefix = "TextBox";

		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (radmin.rIp != null)
			tbIp.Text = radmin.rIp;
		else
			tbIp.Text = "";

		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);
		if (radmin.rPort != null)
			tbPort.Text = radmin.rPort;
		else
			tbPort.Text = "";

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);
		if (radmin.rUsername != null)
			tbUsername.Text = radmin.rUsername;
		else
			tbUsername.Text = "";

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		if (radmin.rPassword != null)
			tbPassword.Text = radmin.rPassword;
		else
			tbPassword.Text = "";
	}
	private void BindMstscValue(Mstsc mstsc, string controlIdInfix)
	{
		string controlIdPrefix = "TextBox";

		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (mstsc.mIp != null)
			tbIp.Text = mstsc.mIp;
		else
			tbIp.Text = "";

		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);
		if (mstsc.mPort != null)
			tbPort.Text = mstsc.mPort;
		else
			tbPort.Text = "";

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);
		if (mstsc.mUsername != null)
			tbUsername.Text = mstsc.mUsername;
		else
			tbUsername.Text = "";

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		if (mstsc.mPassword != null)
			tbPassword.Text = mstsc.mPassword;
		else
			tbPassword.Text = "";
	}
	private void BindTtvncValue(Ttvnc ttvnc, string controlIdInfix)
	{
		string controlIdPrefix = "TextBox";

		string tbCodeId = controlIdPrefix + controlIdInfix + "Code";
		TextBox tbCode = (TextBox)FindControl(tbCodeId);
		string tbAssistantModeId = controlIdPrefix + controlIdInfix + "AssistantMode";
		TextBox tbAssistantMode = (TextBox)FindControl(tbAssistantModeId);
		if (ttvnc.tCode != null)
		{
			tbCode.Text = ttvnc.tCode;
			tbAssistantMode.Text = ttvnc.tAssistantMode.ToString();
		}
		else
		{
			tbCode.Text = "";
			tbAssistantMode.Text = "";
		}
	}
	private void BindTeamviewerValue(Teamviewer teamviewer, string controlIdInfix)
	{
		string controlIdPrefix = "TextBox";

		string tbIdId = controlIdPrefix + controlIdInfix + "Id";
		TextBox tbId = (TextBox)FindControl(tbIdId);
		string tbAssistantTypeId = controlIdPrefix + controlIdInfix + "AssistantType";
		TextBox tbAssistantType = (TextBox)FindControl(tbAssistantTypeId);
		if (teamviewer.tTeamviewerId != null)
		{
			tbId.Text = teamviewer.tTeamviewerId;
			tbAssistantType.Text = teamviewer.tAssistantType.ToString();
		}
		else
		{	
			tbId.Text = "";
			tbAssistantType.Text = "";
		}
		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		if (teamviewer.tPassword != null)
			tbPassword.Text = teamviewer.tPassword;
		else
			tbPassword.Text = "";
	}
	private void BindRemotelyanywhereValue(Remotelyanywhere remotelyanywhere, string controlIdInfix)
	{
		string controlIdPrefix = "TextBox";

		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (remotelyanywhere.rIp != null)
			tbIp.Text = remotelyanywhere.rIp;
		else
			tbIp.Text = "";

		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);
		if (remotelyanywhere.rPort != null)
			tbPort.Text = remotelyanywhere.rPort;
		else
			tbPort.Text = "";

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);
		if (remotelyanywhere.rUsername != null)
			tbUsername.Text = remotelyanywhere.rUsername;
		else
			tbUsername.Text = "";

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		if (remotelyanywhere.rPassword != null)
			tbPassword.Text = remotelyanywhere.rPassword;
		else
			tbPassword.Text = "";
	}

	private void SetRadioButtonSelected(CafeInformation ci)
	{
		if (ci == null)
			return;
		List<string> serverTypes = new List<string>()
	        {
	                "main_server", "secondary_server", "cash_register_server", "movie_server", "router_server", 
	        };

		foreach (string serverType in serverTypes)
		{
			string serverColumnName = NameHelper.GetServerTypeColumnName(serverType);

			string remoteClientName = GetRemoteClientName(ci, serverType);
			RadioButtonList rbl = GetRadioButtonList(serverType);
			rbl.SelectedValue = remoteClientName;
		}
	}

	private string GetRemoteClientName(CafeInformation ci, string serverType)
	{
		string remoteClientName = null;
		switch (serverType)
		{
			case "main_server":
				remoteClientName = ci.CiMainServerType;
				break;
			case "secondary_server":
				remoteClientName = ci.CiSecondaryServerType;
				break;
			case "cash_register_server":
				remoteClientName = ci.CiCashRegisterServerType;
				break;
			case "movie_server":
				remoteClientName = ci.CiMovieServerType;
				break;
			case "router_server":
				remoteClientName = ci.CiRouterServerType;
				break;
		}
		return remoteClientName;
	}
	private void DisplayPageControls(bool visible)
	{
		List<string> serverTypes = new List<string>()
		{
			"main_server", "secondary_server", "cash_register_server", "movie_server", "router_server", 
		};
		foreach (string serverType in serverTypes)
		{
			string radioButtonListId = "RadioButtonList" + NameHelper.GetControlIdInfix(serverType) + "RemoteType";
			RadioButtonList rbl = (RadioButtonList)FindControl(radioButtonListId);
			string remoteClientType = rbl.SelectedValue;
			if (remoteClientType.Length > 0)
			{
				DisplayControls(serverType, NameHelper.GetControlIdSuffices(remoteClientType), true);
			}
		}
	}

	private CafeInformation GetCafeInformation(string id)
	{
		ICafeInformationManager ciManager = GetCafeInformationManager(false);
		CafeInformation ci = null;
		try
		{
			ci = ciManager.GetById(id);
		}
		catch (System.Exception ex)
		{
		}
		finally
		{
			ciManager.Dispose();
		}
		return ci;
	}
	private void FillCafeInformationControls(CafeInformation ci)
	{
		if (ci == null)
			return;
		if (ci.Id != null)
			this.TextBoxCafeId.Text = ci.Id;
		if (ci.CiName != null)
			this.TextBoxCafeName.Text = ci.CiName;
		if (ci.CiTelephone != null)
			this.TextBoxTelephone.Text = ci.CiTelephone;
		if (ci.CiContact != null)
			this.TextBoxContact.Text = ci.CiContact;
		if (ci.CiMobile != null)
			this.TextBoxMobile.Text = ci.CiMobile;
	}

	private RadioButtonList GetRadioButtonList(string serverType)
	{
		RadioButtonList rbl = null;
		switch (serverType)
		{
			case "main_server":
				rbl = this.RadioButtonListMainServerRemoteType;
				break;
			case "secondary_server": 
				rbl = this.RadioButtonListSecondaryServerRemoteType;
				break;
			case "cash_register_server":
				rbl = this.RadioButtonListCashRegisterServerRemoteType;
				break;
			case "movie_server":
				rbl = this.RadioButtonListMovieServerRemoteType;
				break;
			case "router_server":
				rbl = this.RadioButtonListRouterServerRemoteType;
				break;
		}
		return rbl;
	}

	private void DisplayControls(string serverType, List<string> controlIdSuffices, bool visible)
	{
		List<string> allControlIdSuffices = new List<string>()
		{
			"Ip", "Port", "Username", "Password",
			"AssistantMode", "Code",
			"Id", "AssistantType",
		};
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		List<string> controlIdPrefices = new List<string>()
		{
			"Label", "TextBox",
		};
		foreach (string p in controlIdPrefices)
		{
			foreach (string s in allControlIdSuffices)
			{
				string controlId = p + controlIdInfix + s;
				if (controlIdSuffices.Contains(s))
				{
					FindControl(controlId).Visible = visible;
				}
				else
				{
					FindControl(controlId).Visible = !visible;
				}
			}
		}
	}
	private ICafeInformationManager GetCafeInformationManager(bool invalidate)
	{
		ICafeInformationManager cafeInformationManager = null;
		if (Session["cafeInformationManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			cafeInformationManager = managerFactory.GetCafeInformationManager();
			Session["cafeInformationManager"] = cafeInformationManager;
		}
		else
			cafeInformationManager = (ICafeInformationManager)Session["cafeInformationManager"];
		return cafeInformationManager;
	}
	private IRadminManager GetRadminManager(bool invalidate)
	{
		IRadminManager radminManager = null;
		if (Session["radminManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			radminManager = managerFactory.GetRadminManager();
			Session["radminManager"] = radminManager;
		}
		else
			radminManager = (IRadminManager)Session["radminManager"];
		return radminManager;
	}

	private IMstscManager GetMstscManager(bool invalidate)
	{
		IMstscManager mstscManager = null;
		if (Session["mstscManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			mstscManager = managerFactory.GetMstscManager();
			Session["mstscManager"] = mstscManager;
		}
		else
			mstscManager = (IMstscManager)Session["mstscManager"];
		return mstscManager;
	}
	private ITtvncManager GetTtvncManager(bool invalidate)
	{
		ITtvncManager ttvncManager = null;
		if (Session["ttvncManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			ttvncManager = managerFactory.GetTtvncManager();
			Session["ttvncManager"] = ttvncManager;
		}
		else
			ttvncManager = (ITtvncManager)Session["ttvncManager"];
		return ttvncManager;
	}
	private ITeamviewerManager GetTeamviewerManager(bool invalidate)
	{
		ITeamviewerManager teamviewerManager = null;
		if (Session["teamviewerManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			teamviewerManager = managerFactory.GetTeamviewerManager();
			Session["teamviewerManager"] = teamviewerManager;
		}
		else
			teamviewerManager = (ITeamviewerManager)Session["teamviewerManager"];
		return teamviewerManager;
	}
	private IRemotelyanywhereManager GetRemotelyanywhereManager(bool invalidate)
	{
		IRemotelyanywhereManager remotelyanywhereManager = null;
		if (Session["remotelyanywhereManager"] == null || invalidate)
		{
			IManagerFactory managerFactory = new ManagerFactory();
			remotelyanywhereManager = managerFactory.GetRemotelyanywhereManager();
			Session["remotelyanywhereManager"] = remotelyanywhereManager;
		}
		else
			remotelyanywhereManager = (IRemotelyanywhereManager)Session["remotelyanywhereManager"];
		return remotelyanywhereManager;
	}

	protected void ButtonSave_Click(object sender, EventArgs e)
	{
		SaveCafeInformation();
		string remoteClientType;
		if (this.CheckBoxEnableMainServer.Checked)
		{
			remoteClientType = this.RadioButtonListMainServerRemoteType.SelectedValue;
			SaveRemoteClient("main_server", remoteClientType);
		}
		
		if (this.CheckBoxEnableSecondaryServer.Checked)
		{
			remoteClientType = this.RadioButtonListSecondaryServerRemoteType.SelectedValue;
			SaveRemoteClient("secondary_server", remoteClientType);
		}
		
		if (this.CheckBoxEnableCashRegisterServer.Checked)
		{
			remoteClientType = this.RadioButtonListCashRegisterServerRemoteType.SelectedValue;
			SaveRemoteClient("cash_register_server", remoteClientType);
		}
		
		if (this.CheckBoxEnableMovieServer.Checked)
		{
			remoteClientType = this.RadioButtonListMovieServerRemoteType.SelectedValue;
			SaveRemoteClient("movie_server", remoteClientType);
		}
		
		if (this.CheckBoxEnableRouterServer.Checked)
		{
			remoteClientType = this.RadioButtonListRouterServerRemoteType.SelectedValue;
			SaveRemoteClient("router_server", remoteClientType);
		}
		Response.Redirect("Default.aspx");
	}

	private void SaveCafeInformation()
	{
		if (this.TextBoxCafeId.Text.Length == 0 || this.TextBoxCafeName.Text.Length == 0)
			return;
		string id = this.TextBoxCafeId.Text;
		ICafeInformationManager manager = GetCafeInformationManager(true);
		try
		{
			bool save = false;
			CafeInformation ci = manager.Session.GetISession().Get<CafeInformation>(id);
			if (ci == null)
			{
				ci = new CafeInformation();
				ci.Id = id;
				save = true;
			}
			ci.CiName = this.TextBoxCafeName.Text;
			ci.CiTelephone = this.TextBoxTelephone.Text;
			ci.CiContact = this.TextBoxContact.Text;
			ci.CiMobile = this.TextBoxMobile.Text;
			if (this.CheckBoxEnableMainServer.Checked)
				ci.CiMainServerType = this.RadioButtonListMainServerRemoteType.SelectedValue;
			else
				ci.CiMainServerType = "";

			if (this.CheckBoxEnableSecondaryServer.Checked)
				ci.CiSecondaryServerType = this.RadioButtonListSecondaryServerRemoteType.SelectedValue;
			else
				ci.CiSecondaryServerType = "";

			if (this.CheckBoxEnableCashRegisterServer.Checked)
				ci.CiCashRegisterServerType = this.RadioButtonListCashRegisterServerRemoteType.SelectedValue;
			else
				ci.CiCashRegisterServerType = "";

			if (this.CheckBoxEnableMovieServer.Checked)
				ci.CiMovieServerType = this.RadioButtonListMovieServerRemoteType.SelectedValue;
			else
				ci.CiMovieServerType = "";

			if (this.CheckBoxEnableRouterServer.Checked)
				ci.CiRouterServerType = this.RadioButtonListRouterServerRemoteType.SelectedValue;
			else
				ci.CiRouterServerType = "";
			//manager.Session.GetISession().Clear();
			if (save)
				manager.Save(ci);
			else
				manager.Update(ci);
			//manager.Session.GetISession().SaveOrUpdateCopy(ci);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex)
		{
			Response.Write("CafeInformation:</br>" + ex.Message);
		}
		finally
		{
			manager.Dispose();
		}
	}
	private void SaveRemoteClient(string serverType, string remoteClientType)
	{
		string id = this.TextBoxCafeId.Text;

		switch (remoteClientType)
		{
			case "radmin":
				SaveRadmin(id, serverType);
				break;
			case "mstsc":
				SaveMstsc(id, serverType);
				break;
			case "ttvnc":
				SaveTtvnc(id, serverType);
				break;
			case "teamviewer":
				SaveTeamviewer(id, serverType);
				break;
			case "remotelyanywhere":
				SaveRemotelyanywhere(id, serverType);
				break;
		}
	}

	private void SaveRadmin(string id, string serverType)
	{
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		string controlIdPrefix = "TextBox";
		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (tbIp.Text.Length == 0)
			return;
		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		
		IRadminManager manager = GetRadminManager(true);
		try
		{
			bool save = false;
			Radmin key = new Radmin();
			key.CiId = id;
			key.CiServerType = serverType;
			Radmin radmin = manager.Session.GetISession().Get<Radmin>(key);
			if (radmin == null)
			{
				radmin = new Radmin();
				radmin.CiId = id;
				radmin.CiServerType = serverType;
				save = true;
			}
			radmin.rIp = tbIp.Text;
			radmin.rPort = tbPort.Text;
			radmin.rUsername = tbUsername.Text;
			radmin.rPassword = tbPassword.Text;
			if (save)
				manager.Save(radmin);
			else
				manager.Update(radmin);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex) { }
		finally
		{
			manager.Dispose();
		}
	}

	private void SaveMstsc(string id, string serverType)
	{
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		string controlIdPrefix = "TextBox";
		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (tbIp.Text.Length == 0)
			return;
		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);

		IMstscManager manager = GetMstscManager(true);
		try
		{
			bool save = false;
			Mstsc key = new Mstsc();
			key.CiId = id;
			key.CiServerType = serverType;
			Mstsc mstsc = manager.Session.GetISession().Get<Mstsc>(key);
			if (mstsc == null)
			{
				mstsc = new Mstsc();
				mstsc.CiId = id;
				mstsc.CiServerType = serverType;
				save = true;
			}
			mstsc.mIp = tbIp.Text;
			mstsc.mPort = tbPort.Text;
			mstsc.mUsername = tbUsername.Text;
			mstsc.mPassword = tbPassword.Text;
			if (save)
				manager.Save(mstsc);
			else
				manager.Update(mstsc);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex) { }
		finally
		{
			manager.Dispose();
		}
	}
	private void SaveTtvnc(string id, string serverType)
	{
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		string controlIdPrefix = "TextBox";
		string tbCodeId = controlIdPrefix + controlIdInfix + "Code";
		TextBox tbCode = (TextBox)FindControl(tbCodeId);
		string tbAssistantModeId = controlIdPrefix + controlIdInfix + "AssistantMode";
		TextBox tbAssistantMode = (TextBox)FindControl(tbAssistantModeId);
		if (tbCode.Text.Length == 0 || tbAssistantMode.Text.Length == 0)
			return;

		ITtvncManager manager = GetTtvncManager(true);
		try
		{
			bool save = false;
			Ttvnc key = new Ttvnc();
			key.CiId = id;
			key.CiServerType = serverType;
			Ttvnc ttvnc = manager.Session.GetISession().Get<Ttvnc>(key);
			if (ttvnc == null)
			{
				ttvnc = new Ttvnc();
				ttvnc.CiId = id;
				ttvnc.CiServerType = serverType;
				save = true;
			}
			ttvnc.tCode = tbCode.Text;
			ttvnc.tAssistantMode = Convert.ToByte(tbAssistantMode.Text);
			if (save)
				manager.Save(ttvnc);
			else
				manager.Update(ttvnc);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex) { }
		finally
		{
			manager.Dispose();
		}
	}

	private void SaveTeamviewer(string id, string serverType)
	{
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		string controlIdPrefix = "TextBox";
	
		string tbIdId = controlIdPrefix + controlIdInfix + "Id";
		TextBox tbId = (TextBox)FindControl(tbIdId);
		if (tbId.Text.Length == 0)
			return;
		
		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);
		if (tbPassword.Text.Length == 0)
			return;

		string tbAssistantTypeId = controlIdPrefix + controlIdInfix + "AssistantType";
		TextBox tbAssistantType = (TextBox)FindControl(tbAssistantTypeId);
		string assistantType = tbAssistantType.Text;
		if (tbAssistantType.Text.Length == 0)
		{
			assistantType = "0";
		}
		ITeamviewerManager manager = GetTeamviewerManager(true);
		try
		{
			bool save = false;
			Teamviewer key = new Teamviewer();
			key.CiId = id;
			key.CiServerType = serverType;
			Teamviewer teamviewer = manager.Session.GetISession().Get<Teamviewer>(key);
			if (teamviewer == null)
			{
				teamviewer = new Teamviewer();
				teamviewer.CiId = id;
				teamviewer.CiServerType = serverType;
				save = true;
			}
			teamviewer.tTeamviewerId = tbId.Text;
			teamviewer.tPassword = tbPassword.Text;
			teamviewer.tAssistantType = Convert.ToInt32(assistantType);
			if (save)
				manager.Save(teamviewer);
			else
				manager.Update(teamviewer);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex) { }
		finally
		{
			manager.Dispose();
		}
	}

	private void SaveRemotelyanywhere(string id, string serverType)
	{
		string controlIdInfix = NameHelper.GetControlIdInfix(serverType);
		string controlIdPrefix = "TextBox";
		string tbIpId = controlIdPrefix + controlIdInfix + "Ip";
		TextBox tbIp = (TextBox)FindControl(tbIpId);
		if (tbIp.Text.Length == 0)
			return;
		string tbPortId = controlIdPrefix + controlIdInfix + "Port";
		TextBox tbPort = (TextBox)FindControl(tbPortId);

		string tbUsernameId = controlIdPrefix + controlIdInfix + "Username";
		TextBox tbUsername = (TextBox)FindControl(tbUsernameId);

		string tbPasswordId = controlIdPrefix + controlIdInfix + "Password";
		TextBox tbPassword = (TextBox)FindControl(tbPasswordId);

		IRemotelyanywhereManager manager = GetRemotelyanywhereManager(true);
		try
		{
			bool save = false;
			Remotelyanywhere key = new Remotelyanywhere();
			key.CiId = id;
			key.CiServerType = serverType;
			Remotelyanywhere remotelyanywhere = manager.Session.GetISession().Get<Remotelyanywhere>(key);
			if (remotelyanywhere == null)
			{
				remotelyanywhere = new Remotelyanywhere();
				remotelyanywhere.CiId = id;
				remotelyanywhere.CiServerType = serverType;
				save = true;
			}
			remotelyanywhere.rIp = tbIp.Text;
			remotelyanywhere.rPort = tbPort.Text;
			remotelyanywhere.rUsername = tbUsername.Text;
			remotelyanywhere.rPassword = tbPassword.Text;
			if (save)
				manager.Save(remotelyanywhere);
			else
				manager.Update(remotelyanywhere);
			manager.Session.CommitChanges();
		}
		catch (System.Exception ex) { }
		finally
		{
			manager.Dispose();
		}
	}
}
