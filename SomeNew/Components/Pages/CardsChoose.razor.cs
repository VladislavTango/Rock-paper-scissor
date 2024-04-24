using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SomeNew.Components.Pages
{
    
    public partial class CardsChoose: ComponentBase
    {
        string? choosenCard = "";

        [Parameter]
        public string? ChoosenCard
        {
            get { return choosenCard; }
            set { choosenCard = value; }
        }
        string GetPage(string item) {
            return $"Images/{item}.png";
        }

        void Choose(string value)
        {
            choosenCard = value;
            ChangeValue(value);
        }

        [Parameter]
        public EventCallback<string> ChoosenCardChanged { get; set; }

        async Task ChangeValue(string e)
        {
            choosenCard = e;
            await ChoosenCardChanged.InvokeAsync(choosenCard);
        }
    }
}
