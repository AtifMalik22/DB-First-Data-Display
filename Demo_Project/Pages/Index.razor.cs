using Demo_Project.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo_Project.Pages
{
    public partial class Index
    {
        v_Accounts _Accounts=new();
        public List<v_Accounts> displayList=new();
        protected override async Task OnInitializedAsync()
        {
            displayList = await accountSerive.GetDataAsync();
        }
        protected async void AddData()
        {
            await accountSerive.InsertData(_Accounts);
            NavigationManager.NavigateTo("", true);
        }
    }
}