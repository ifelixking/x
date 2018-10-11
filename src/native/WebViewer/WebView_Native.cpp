#include "stdafx.h"
#include "WebView_Native.h"
#include "WebView.h"

namespace WebViewer {

	WebView_Native::WebView_Native(void * host, QWebEngineView * view)
		: m_host(host)
		, m_view(view)
	{
		connect(m_view->page(), SIGNAL(loadFinished(bool)), this, SLOT(loadFinished(bool)));
		connect(m_view->page(), SIGNAL(contentsSizeChanged(const QSizeF &)), this, SLOT(contentsSizeChanged(const QSizeF &)));
		connect(m_view->page(), SIGNAL(geometryChangeRequested(const QRect &)), this, SLOT(geometryChangeRequested(const QRect &)));
		connect(m_view->page(), SIGNAL(linkHovered(const QString &)), this, SLOT(linkHovered(const QString &)));
		connect(m_view->page(), SIGNAL(loadProgress(int)), this, SLOT(loadProgress(int)));
		connect(m_view->page(), SIGNAL(loadStarted()), this, SLOT(loadStarted()));
		connect(m_view->page(), SIGNAL(scrollPositionChanged(const QPointF &)), this, SLOT(scrollPositionChanged(const QPointF &)));
		connect(m_view->page(), SIGNAL(selectionChanged()), this, SLOT(selectionChanged()));
		connect(m_view->page(), SIGNAL(titleChanged(const QString &)), this, SLOT(titleChanged(const QString &)));
		connect(m_view->page(), SIGNAL(urlChanged(const QUrl &)), this, SLOT(urlChanged(const QUrl &)));
		connect(m_view->page(), SIGNAL(windowCloseRequested()), this, SLOT(windowCloseRequested()));

		m_jsContext = new JsContext(this);
		m_webChannel = new QWebChannel(this);
		m_webChannel->registerObject("context", m_jsContext);
		m_view->page()->setWebChannel(m_webChannel);
		connect(m_jsContext, &JsContext::recvdMsg, this, [this](const QString& msg) {
			qDebug()<<msg;
		});
	}

	WebView_Native::~WebView_Native() {
		QObject::disconnect(m_view->page(), 0, this, 0);
	}

	void WebView_Native::loadFinished(bool ok) {
		//auto resourceAssembly = Reflection::Assembly::GetExecutingAssembly();
		//auto resourceName = resourceAssembly->GetName()->Name + ".Resource";
		//auto resourceManager = gcnew Resources::ResourceManager(resourceName, resourceAssembly);
		//auto initScript_1 = cli::safe_cast<String^>(resourceManager->GetObject("qwebchannel"));
		//pin_ptr<const WCHAR> str_initScript_1 = PtrToStringChars(initScript_1);
		//m_view->page()->runJavaScript(QString::fromStdWString(str_initScript_1), [ok, this](const QVariant & result){
			WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
			webView->emitLoadFinish(ok);
		//});
	}
	void WebView_Native::contentsSizeChanged(const QSizeF &size) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitContentsSizeChanged(size);
	}
	void WebView_Native::geometryChangeRequested(const QRect &geom) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitGeometryChangeRequested(geom);
	}
	void WebView_Native::linkHovered(const QString &url) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitLinkHovered(url);
	}
	void WebView_Native::loadProgress(int progress) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitLoadProgress(progress);
	}
	void WebView_Native::loadStarted() {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitLoadStarted();
	}
	void WebView_Native::scrollPositionChanged(const QPointF &position) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitScrollPositionChanged(position);
	}
	void WebView_Native::selectionChanged() {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitSelectionChanged();
	}
	void WebView_Native::titleChanged(const QString &title) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitTitleChanged(title);
	}
	void WebView_Native::urlChanged(const QUrl &url) {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitUrlChanged(url);
	}
	void WebView_Native::windowCloseRequested() {
		WebView ^ webView = *((gcroot<WebView ^> *)this->m_host);
		webView->emitWindowCloseRequested();
	}

	// ===================

	void JsContext::onMsg(const QString &msg)
	{
		emit recvdMsg(msg);
	}

}