// (C) Copyright Gert-Jan de Vos and Jan Wilmans 2013.
// Distributed under the Boost Software License, Version 1.0.
// (See accompanying file LICENSE_1_0.txt or copy at 
// http://www.boost.org/LICENSE_1_0.txt)

// Repository at: https://github.com/djeedjay/DebugViewPP/

#pragma once

#include <boost/thread.hpp>
#include "PipeReader.h"
#include "Process.h"

namespace fusion {
namespace debugviewpp {

class ILineBuffer;

class FileReader : public LogSource
{
public:
	explicit FileReader(ILineBuffer& linebuffer, const std::wstring& filename);
    ~FileReader();

	virtual bool AtEnd() const;
	virtual HANDLE GetHandle() const;
	virtual void Notify();
	virtual void AddLine(const std::string line);
	virtual std::wstring GetProcessName(HANDLE handle) const;

protected:
	std::string m_filename;	
	std::string m_name;	

private:
	void ReadUntilEof();
    void Abort();

    bool m_end;
	ChangeNotificationHandle m_handle;
	std::ifstream m_ifstream;
	std::string m_filenameOnly;
};

class DBLogReader : public FileReader
{
public:
	explicit DBLogReader(ILineBuffer& linebuffer, const std::wstring& filename);
	virtual void AddLine(const std::string data);
private:
	FILETIME m_time;
};

} // namespace debugviewpp 
} // namespace fusion
