#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import <QuartzCore/QuartzCore.h>
#include <stdlib.h>
#include <string.h>
#include <atomic>
#import "UnityInterface.h"

@interface ErnyRaceBrowser : WKWebView
@property(nonatomic, strong) UIBezierPath *cutout;
@property(nonatomic, copy) NSString *landingAddress;
@property(nonatomic) BOOL measuringCookie;
@end

@implementation ErnyRaceBrowser
- (BOOL)pointInside:(CGPoint)point withEvent:(UIEvent *)event {
    return ![self.cutout containsPoint:point] && [super pointInside:point withEvent:event];
}
@end

@interface ErnyRaceNavigation : NSObject <WKNavigationDelegate>
@end

@implementation ErnyRaceNavigation
- (void)webView:(WKWebView *)webView didCommitNavigation:(WKNavigation *)navigation {
    ErnyRaceBrowser *raceBrowser = (ErnyRaceBrowser *)webView;
    if (!raceBrowser.landingAddress) raceBrowser.landingAddress = webView.URL.absoluteString;
}

- (void)webView:(WKWebView *)webView
    decidePolicyForNavigationAction:(WKNavigationAction *)action
                    decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler {
    if (![action.request.URL.scheme.lowercaseString isEqualToString:@"https"]) {
        decisionHandler(WKNavigationActionPolicyCancel);
        return;
    }
    if (!action.targetFrame) {
        [webView loadRequest:action.request];
        decisionHandler(WKNavigationActionPolicyCancel);
        return;
    }
    decisionHandler(WKNavigationActionPolicyAllow);
}
@end

static ErnyRaceBrowser *browser;
static ErnyRaceNavigation *navigation;
static std::atomic<float> cookieInset{0};

extern "C" float ErnyRaceWebMeasureCookieBanner(const char *script) {
    NSString *source = [NSString stringWithUTF8String:script];
    dispatch_async(dispatch_get_main_queue(), ^{
        ErnyRaceBrowser *view = browser;
        if (!view || view.measuringCookie) return;
        view.measuringCookie = YES;
        [view evaluateJavaScript:source completionHandler:^(id value, NSError *error) {
            view.measuringCookie = NO;
            if (view == browser)
                cookieInset.store([value isKindOfClass:[NSNumber class]] ? [value floatValue] : 0);
        }];
    });
    return cookieInset.load();
}

extern "C" char *ErnyRaceWebCopyAddress(int landing) {
    __block char *result = nullptr;
    void (^readAddress)(void) = ^{
        NSString *address = landing ? browser.landingAddress : browser.URL.absoluteString;
        if (address) result = strdup(address.UTF8String);
    };
    if ([NSThread isMainThread]) readAddress();
    else dispatch_sync(dispatch_get_main_queue(), readAddress);
    return result;
}

extern "C" void ErnyRaceWebFreeAddress(char *address) { free(address); }

extern "C" void ErnyRaceWebOpen(const char *url) {
    NSString *address = [NSString stringWithUTF8String:url];
    cookieInset.store(0);
    dispatch_async(dispatch_get_main_queue(), ^{
        [browser stopLoading];
        [browser removeFromSuperview];
        UIView *parent = UnityGetGLViewController().view;
        browser = [[ErnyRaceBrowser alloc] initWithFrame:CGRectZero];
        navigation = [[ErnyRaceNavigation alloc] init];
        browser.navigationDelegate = navigation;
        browser.hidden = YES;
        [parent addSubview:browser];
        [browser loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:address]]];
    });
}

extern "C" void ErnyRaceWebLayout(float left, float top, float right, float bottom,
    float cutLeft, float cutTop, float cutRight, float cutBottom) {
    dispatch_async(dispatch_get_main_queue(), ^{
        CGSize size = browser.superview.bounds.size;
        browser.frame = CGRectMake(left * size.width, top * size.height,
            (right - left) * size.width, (bottom - top) * size.height);
        CGRect hole = CGRectMake((cutLeft - left) * size.width, (cutTop - top) * size.height,
            (cutRight - cutLeft) * size.width, (cutBottom - cutTop) * size.height);
        if (CGRectIsEmpty(CGRectIntersection(browser.bounds, hole))) {
            browser.cutout = nil;
            browser.layer.mask = nil;
        } else {
            browser.cutout = [UIBezierPath bezierPathWithRoundedRect:hole
                cornerRadius:MIN(hole.size.width, hole.size.height) * .5];
            UIBezierPath *path = [UIBezierPath bezierPathWithRect:browser.bounds];
            [path appendPath:browser.cutout];
            CAShapeLayer *mask = [CAShapeLayer layer];
            mask.frame = browser.bounds;
            mask.path = path.CGPath;
            mask.fillRule = kCAFillRuleEvenOdd;
            browser.layer.mask = mask;
        }
        browser.hidden = NO;
    });
}

extern "C" void ErnyRaceWebVisible(bool visible) {
    dispatch_async(dispatch_get_main_queue(), ^{ browser.hidden = !visible; });
}

extern "C" void ErnyRaceWebClose() {
    dispatch_async(dispatch_get_main_queue(), ^{
        cookieInset.store(0);
        [browser stopLoading];
        [browser removeFromSuperview];
        browser = nil;
        navigation = nil;
    });
}
