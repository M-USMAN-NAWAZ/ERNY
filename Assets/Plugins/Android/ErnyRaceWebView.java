package com.blingar;

import android.app.Activity;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Path;
import android.graphics.RectF;
import android.graphics.Region;
import android.view.MotionEvent;
import android.view.View;
import android.webkit.WebResourceRequest;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.FrameLayout;

public final class ErnyRaceWebView {
    private static RaceBrowser browser;
    private static FrameLayout container;
    private static volatile String currentAddress, landingAddress;
    private static volatile float cookieInset;

    public static float cookieBannerInset() { return cookieInset; }

    public static void measureCookieBanner(Activity activity, String script) {
        activity.runOnUiThread(() -> {
            RaceBrowser view = browser;
            if (view == null || view.measuringCookie) return;
            view.measuringCookie = true;
            view.evaluateJavascript(script, value -> {
                view.measuringCookie = false;
                if (view != browser) return;
                try { cookieInset = Float.parseFloat(value); }
                catch (NumberFormatException | NullPointerException ignored) { cookieInset = 0; }
            });
        });
    }

    public static String address(boolean landing) {
        return landing ? landingAddress : currentAddress;
    }

    public static void open(Activity activity, String url) {
        activity.runOnUiThread(() -> {
            closeView();
            currentAddress = url;
            landingAddress = null;
            container = activity.findViewById(android.R.id.content);
            browser = new RaceBrowser(activity);
            browser.setBackgroundColor(Color.WHITE);
            browser.setVisibility(View.INVISIBLE);
            WebSettings settings = browser.getSettings();
            settings.setJavaScriptEnabled(true);
            settings.setDomStorageEnabled(true);
            settings.setAllowFileAccess(false);
            settings.setAllowContentAccess(false);
            settings.setMixedContentMode(WebSettings.MIXED_CONTENT_NEVER_ALLOW);
            browser.setWebViewClient(new WebViewClient() {
                @Override
                public boolean shouldOverrideUrlLoading(WebView view, WebResourceRequest request) {
                    return !"https".equalsIgnoreCase(request.getUrl().getScheme());
                }

                @Override
                public void doUpdateVisitedHistory(WebView view, String url, boolean reload) {
                    if (view == browser) currentAddress = url;
                }

                @Override
                public void onPageFinished(WebView view, String url) {
                    if (view == browser && landingAddress == null) landingAddress = view.getUrl();
                }
            });
            container.addView(browser, new FrameLayout.LayoutParams(0, 0));
            browser.loadUrl(url);
        });
    }

    public static void layout(Activity activity, float left, float top, float right, float bottom,
                              float cutLeft, float cutTop, float cutRight, float cutBottom) {
        activity.runOnUiThread(() -> {
            if (browser == null) return;
            int w = container.getWidth(), h = container.getHeight();
            int x = Math.round(left * w), y = Math.round(top * h);
            FrameLayout.LayoutParams bounds = new FrameLayout.LayoutParams(
                Math.max(0, Math.round(right * w) - x), Math.max(0, Math.round(bottom * h) - y));
            bounds.leftMargin = x;
            bounds.topMargin = y;
            browser.setCutout((int)Math.floor(cutLeft * w) - x, (int)Math.floor(cutTop * h) - y,
                (int)Math.ceil(cutRight * w) - x, (int)Math.ceil(cutBottom * h) - y);
            browser.setLayoutParams(bounds);
            browser.invalidate();
            browser.setVisibility(View.VISIBLE);
        });
    }

    public static void visible(Activity activity, boolean visible) {
        activity.runOnUiThread(() -> {
            if (browser != null) browser.setVisibility(visible ? View.VISIBLE : View.INVISIBLE);
        });
    }

    public static void close(Activity activity) {
        activity.runOnUiThread(ErnyRaceWebView::closeView);
    }

    private static void closeView() {
        cookieInset = 0;
        if (browser == null) return;
        container.removeView(browser);
        browser.stopLoading();
        browser.destroy();
        browser = null;
        container = null;
    }

    private static final class RaceBrowser extends WebView {
        boolean measuringCookie;
        final Path cutout = new Path();
        final Region cutoutHit = new Region();

        RaceBrowser(Activity activity) { super(activity); }

        void setCutout(int left, int top, int right, int bottom) {
            cutout.reset();
            if (right > left && bottom > top) {
                float radius = Math.min(right - left, bottom - top) * .5f;
                cutout.addRoundRect(new RectF(left, top, right, bottom), radius, radius, Path.Direction.CW);
            }
            cutoutHit.setPath(cutout, new Region(left, top, right, bottom));
        }

        @Override
        @SuppressWarnings("deprecation")
        public void draw(Canvas canvas) {
            int saved = canvas.save();
            if (android.os.Build.VERSION.SDK_INT >= 26) canvas.clipOutPath(cutout);
            else canvas.clipPath(cutout, Region.Op.DIFFERENCE);
            super.draw(canvas);
            canvas.restoreToCount(saved);
        }

        @Override
        public boolean dispatchTouchEvent(MotionEvent event) {
            if (event.getActionMasked() == MotionEvent.ACTION_DOWN
                && cutoutHit.contains((int)event.getX(), (int)event.getY())) return false;
            return super.dispatchTouchEvent(event);
        }
    }
}
