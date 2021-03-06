// (C) Copyright Gert-Jan de Vos and Jan Wilmans 2013.
// Distributed under the Boost Software License, Version 1.0.
// (See accompanying file LICENSE_1_0.txt or copy at 
// http://www.boost.org/LICENSE_1_0.txt)

// Repository at: https://github.com/djeedjay/DebugViewPP/

#include "stdafx.h"
#include "CobaltFusion/AtlWinExt.h"
#include "CobaltFusion/Str.h"
#include "Win32/Utilities.h"
#include "HistoryDlg.h"

namespace fusion {
namespace debugviewpp {

BEGIN_MSG_MAP2(CHistoryDlg)
	MSG_WM_INITDIALOG(OnInitDialog)
	COMMAND_ID_HANDLER_EX(IDC_UNLIMITED, OnUnlimited)
	COMMAND_ID_HANDLER_EX(IDCANCEL, OnCancel)
	COMMAND_ID_HANDLER_EX(IDOK, OnOk)
	REFLECT_NOTIFICATIONS()
END_MSG_MAP()

CHistoryDlg::CHistoryDlg(int historySize, bool unlimited) :
	m_historySize(historySize),
	m_unlimited(unlimited)
{
}

void CHistoryDlg::OnException()
{
	MessageBox(L"Unknown Exception", LoadString(IDR_APPNAME).c_str(), MB_ICONERROR | MB_OK);
}

void CHistoryDlg::OnException(const std::exception& ex)
{
	MessageBox(WStr(ex.what()).c_str(), LoadString(IDR_APPNAME).c_str(), MB_ICONERROR | MB_OK);
}

BOOL CHistoryDlg::OnInitDialog(CWindow /*wndFocus*/, LPARAM /*lInitParam*/)
{
	SetDlgItemInt(IDC_HISTORY, m_historySize);

	CButton unlimited(GetDlgItem(IDC_UNLIMITED));
	unlimited.SetCheck(m_unlimited);
	UpdateUi();

	CenterWindow(GetParent());

	return TRUE;
}

void CHistoryDlg::OnUnlimited(UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/)
{
	CButton unlimited(GetDlgItem(IDC_UNLIMITED));
	m_unlimited = unlimited.GetCheck() == BST_CHECKED;
	UpdateUi();
}

void CHistoryDlg::UpdateUi()
{
	GetDlgItem(IDC_HISTORY).EnableWindow(!m_unlimited);
}

void CHistoryDlg::OnCancel(UINT /*uNotifyCode*/, int nID, CWindow /*wndCtl*/)
{
	EndDialog(nID);
}

void CHistoryDlg::OnOk(UINT /*uNotifyCode*/, int nID, CWindow /*wndCtl*/)
{
	m_historySize = GetDlgItemInt(IDC_HISTORY);
//	m_unlimited = fusion::GetDlgItemText(*this, IDC_ARGUMENTS);

	EndDialog(nID);
}

int CHistoryDlg::GetHistorySize() const
{
	return m_historySize;
}

} // namespace debugviewpp 
} // namespace fusion
