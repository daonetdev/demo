using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapBlazor.Maui1.Shared.Pages
{
    public partial class Confirm
    {
        private string BindValue { get; set; } = string.Empty;

        [Parameter]
        public Func<Task>? OnClick { get; set; }


        /// <summary>
        /// 关闭弹窗
        /// </summary>
        [Parameter]
        public Func<Task>? OnClose { get; set; }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        [Parameter]
        public Func<string, Task>? OnPass { get; set; }


        /// <summary>
        /// 输入控件绑定
        /// </summary>
        private BootstrapInput<string>? InputElemont { get; set; }





        /// <summary>
        ///级联参数赋值--Func绑定方法
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InputElemont.SelectAllTextAsync();
                await InvokeAsync(StateHasChanged);
            }

            await base.OnAfterRenderAsync(firstRender);
        }


        private async Task OnEnterAsync(string sKey)
        {
            await OnClickAsync();
        }




        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task OnClickAsync()
        {
            try
            {
                if (BindValue == "123580")
                {
                    if (OnClose != null) await OnClose!.Invoke();
                    if (OnPass != null) await OnPass!.Invoke("OK");
                    return;
                }

                await InputElemont.SelectAllTextAsync();
            }
            catch (Exception e)
            { }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
