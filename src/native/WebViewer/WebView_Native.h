#pragma once

#include <QtWebEngineWidgets/QWebEngineView>

namespace WebViewer {

	class WebView_Native : public QObject {
		Q_OBJECT
	public:
		WebView_Native(void * host, QWebEngineView * view);
		~WebView_Native();

	public slots :
		void loadFinished(bool ok);
		void contentsSizeChanged(const QSizeF &size);
		void geometryChangeRequested(const QRect &geom);
		void linkHovered(const QString &url);
		void loadProgress(int progress);
		void loadStarted();
		void scrollPositionChanged(const QPointF &position);
		void selectionChanged();
		void titleChanged(const QString &title);
		void urlChanged(const QUrl &url);
		void windowCloseRequested();

	private:
		void * m_host;
		QWebEngineView * m_view;
	};

}