#pragma checksum "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1b982733badd7e55a879565c2d69fb6e92cbacaf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Web14.Pages.Pages_Index), @"mvc.1.0.razor-page", @"/Pages/Index.cshtml")]
namespace Web14.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/_ViewImports.cshtml"
using Web14;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
using Web14.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1b982733badd7e55a879565c2d69fb6e92cbacaf", @"/Pages/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ce6b5890645e4dfcc01c55b92bb21b7c47970502", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 10 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
  
    ViewData["Title"] = "Home page";
    var a = SignInManager.IsSignedIn(User) ? "Đăng nhập rồi!" : "Chưa đăng nhập?";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <h1 class=\"display-4\">Welcome</h1>\r\n    <h1>");
#nullable restore
#line 17 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
   Write(a);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n");
#nullable restore
#line 19 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
     if (SignInManager.IsSignedIn(User))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h2>");
#nullable restore
#line 21 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
       Write(UserManager.GetUserName(User));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n");
#nullable restore
#line 22 "/Users/huynhngocson/Desktop/xtlab/LearnWeb/Web14/Pages/Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <p>Learn about <a href=\"https://docs.microsoft.com/aspnet/core\">building Web apps with ASP.NET Core</a>.</p>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<AppUser> UserManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<AppUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591