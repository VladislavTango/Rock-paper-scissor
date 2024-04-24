using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeNew.Components.Pages
{
    public partial class Cards : ComponentBase
    {
        [Parameter] public string? ChoosenCard  { get; set; }

        protected override void OnParametersSet()
        {
            ShowCard();
        }
        string ShowCard() {
            return $"Images/{ChoosenCard}.png";
        }
    }
}
