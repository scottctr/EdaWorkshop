// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace ThreeAmigosHealthServer.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using ThreeAmigosHealthServer;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using ThreeAmigosHealthServer.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\_Imports.razor"
using MudBlazor;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/dashboard")]
    public partial class Dashboard : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 9 "C:\Users\scott\Source\Repos\EdaWorkshop2\tahServer\ThreeAmigosHealthServer\Pages\Dashboard.razor"
       
    private int Index = -1; //default value cannot be 0 -> first selectedindex is 0.
    int dataSize = 4;
    double[] data = { 77, 25, 20, 5 };
    string[] labels = { "Uranium", "Plutonium", "Thorium", "Caesium", "Technetium", "Promethium",
                        "Polonium", "Astatine", "Radon", "Francium", "Radium", "Actinium", "Protactinium",
                        "Neptunium", "Americium", "Curium", "Berkelium", "Californium", "Einsteinium", "Mudblaznium" };

    Random random = new Random();

    void RandomizeData()
    {
        var new_data = new double[dataSize];
        for (int i = 0; i < new_data.Length; i++)
            new_data[i] = random.NextDouble() * 100;
        data = new_data;
        StateHasChanged();
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591