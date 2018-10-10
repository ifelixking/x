#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;



namespace WebViewer {
	public delegate void LoadFinishedHandler(System::Object ^ sender, bool ok);
	public delegate void ContentsSizeChangedHandler(System::Object ^ sender, double width, double height);
	public delegate void GeometryChangeRequestedHandler(System::Object ^ sender, int x, int y, int width, int height);
	public delegate void LinkHoveredHandler(System::Object ^ sender, System::String ^ url);
	public delegate void LoadProgressHandler(System::Object ^ sender, int progress);
	public delegate void LoadStartedHandler(System::Object ^ sender);
	public delegate void ScrollPositionChangedHandler(System::Object ^ sender, double x, double y);
	public delegate void SelectionChangedHandler(System::Object ^ sender);
	public delegate void TitleChangedHandler(System::Object ^ sender, System::String ^ title);
	public delegate void UrlChangedHandler(System::Object ^ sender, System::String ^ url);
	public delegate void WindowCloseRequestedHandler(System::Object ^ sender);

	public delegate void ScriptResultHandler(String^ str);

	public ref class WebView : public System::Windows::Forms::UserControl
	{
	public:
		WebView(void);

		void SetUrl(System::String ^ url);
		void RunJavaScript(System::String ^ script, ScriptResultHandler ^ handler);

	public:
		event LoadFinishedHandler ^ LoadFinished;
		event ContentsSizeChangedHandler ^ ContentsSizeChanged;
		event GeometryChangeRequestedHandler ^ GeometryChangeRequested;
		event LinkHoveredHandler ^ LinkHovered;
		event LoadProgressHandler ^ LoadProgress;
		event LoadStartedHandler ^ LoadStarted;
		event ScrollPositionChangedHandler ^ ScrollPositionChanged;
		event SelectionChangedHandler ^ SelectionChanged;
		event TitleChangedHandler ^ TitleChanged;
		event UrlChangedHandler ^ UrlChanged;
		event WindowCloseRequestedHandler ^ WindowCloseRequested;

	internal:
		void emitLoadFinish(bool ok) { LoadFinished(this, ok); }
		void emitContentsSizeChanged(const QSizeF & size) { ContentsSizeChanged(this, size.width(), size.height()); }
		void emitGeometryChangeRequested(const QRect & geom) { GeometryChangeRequested(this, geom.x(), geom.y(), geom.width(), geom.height()); }
		void emitLinkHovered(const QUrl & url) { LinkHovered(this, gcnew System::String(url.toString().toStdWString().c_str())); }
		void emitLoadProgress(int progress) { LoadProgress(this, progress); }
		void emitLoadStarted() { LoadStarted(this); }
		void emitScrollPositionChanged(const QPointF & position) { ScrollPositionChanged(this, position.x(), position.y()); }
		void emitSelectionChanged() { SelectionChanged(this); }
		void emitTitleChanged(const QString & title) { TitleChanged(this, gcnew System::String(title.toStdWString().c_str())); }
		void emitUrlChanged(const QUrl & url) { UrlChanged(this, gcnew System::String(url.toString().toStdWString().c_str())); }
		void emitWindowCloseRequested() { WindowCloseRequested(this); }

	protected:
		~WebView();
		virtual void OnResize(System::EventArgs ^ e) override;
		// virtual void OnGotFocus(System::EventArgs ^ e) override;

	private:
		System::ComponentModel::Container ^components;
		QWebEngineView * m_view;
		class WebView_Native * m_native;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		void InitializeComponent(void)
		{
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
		}
#pragma endregion
	};
}
