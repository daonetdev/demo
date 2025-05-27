var DemoAdminBlazorJS = {
    sidebarToggle: function () {
        var bd = $(document.body)
        if (!bd.hasClass('sidebar-open') && !bd.hasClass('sidebar-collapse') || bd.hasClass('sidebar-open'))
            bd.addClass('sidebar-collapse').removeClass('sidebar-open')
        else
            bd.removeClass('sidebar-collapse').addClass('sidebar-open')
    },
    modalShow: function (id, dotnetRef) {
        var ele = $('#' + id);
        ele.modal('show');
        ele.on('hidden.bs.modal', e => {
            if (id != e.target.id) return
            dotnetRef.invokeMethodAsync('ModalOnClose')
        })
    },
    setCookie: function (name, value, expireDays) {
        var cookie = decodeURIComponent(name) + "=" + decodeURIComponent(value);
        if (expireDays > 0) {
            var d = new Date();
            d.setTime(d.getTime() + (expireDays * 24 * 60 * 60 * 1000));
            cookie += ";expires=" + d.toUTCString();
        }
        cookie += ";path=/";
        document.cookie = cookie;
    }
};