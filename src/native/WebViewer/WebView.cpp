#include "stdafx.h"
#include "WebView.h"
#include "WebView_Native.h"

QWebEngineView * createWebEngineView(int width, int height);
QApplication * g_app = NULL;


namespace WebViewer {

	WebView::WebView(void)
	{
		InitializeComponent();

		if (g_app == NULL) { g_app = new QApplication(__argc, __argv); }
		m_view = new QWebEngineView();
		m_view->setWindowFlags(Qt::FramelessWindowHint);
		m_view->setGeometry(0, 0, this->Width, this->Height);
		m_view->show();
		HWND hwnd = (HWND)m_view->winId();
		SetParent(hwnd, (HWND)this->Handle.ToPointer());

		m_native = new WebView_Native(new gcroot<WebView ^>(this), m_view);
	}

	WebView::~WebView()
	{
		delete m_native;
		m_view->close();
		delete m_view; m_view = NULL;
		delete g_app; g_app = NULL;		
		if (components) { delete components; }
	}

	void WebView::OnResize(System::EventArgs ^ e) {
		UserControl::OnResize(e);
		m_view->setGeometry(0, 0, this->Width, this->Height);
	}

	//void WebViewer::OnGotFocus(System::EventArgs ^ e) {
	//	// UserControl::OnGotFocus(e);
	//	m_view->setFocus();
	//}

	void WebView::SetUrl(System::String ^ url) {
		pin_ptr<const WCHAR> str1 = PtrToStringChars(url);
		m_view->setUrl(QUrl(QString::fromStdWString(str1)));
	}

	void runJavaScript(QWebEnginePage * page, const QString & script, WebViewer::ScriptResultHandler ^ handler) {
		auto cb = new gcroot<WebViewer::ScriptResultHandler ^>(handler);
		page->runJavaScript(script, [cb](const QVariant &v) {
			std::wstring msg = v.toString().toStdWString();
			WebViewer::ScriptResultHandler ^ theHandler = *cb;
			theHandler(gcnew System::String(msg.c_str()));
			delete cb;
		});
	}

	void WebView::RunJavaScript(System::String ^ script, ScriptResultHandler ^ handler) {
		pin_ptr<const WCHAR> str1 = PtrToStringChars(script);
		runJavaScript(m_view->page(), QString::fromStdWString(str1), handler);

		
	}

}