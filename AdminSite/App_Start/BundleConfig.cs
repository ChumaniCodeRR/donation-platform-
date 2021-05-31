using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLayout(bundles);

            RegisterShared(bundles);

            RegisterAccount(bundles);

            RegisterHome(bundles);

            RegisterCharts(bundles);

            RegisterWidgets(bundles);

            RegisterElements(bundles);

            RegisterForms(bundles);

            RegisterTables(bundles);

            RegisterCalendar(bundles);

            RegisterMailbox(bundles);

            RegisterExamples(bundles);

            RegisterDocumentation(bundles);
            //BundleTable.EnableOptimizations = true;
        }

        private static void RegisterDocumentation(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Documentation/menu").Include(
                "~/Scripts/Documentation/Documentation-menu.js"));
        }

        private static void RegisterExamples(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Examples/Blank/menu").Include(
                "~/Scripts/Examples/Blank-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Invoice/menu").Include(
                "~/Scripts/Examples/Invoice-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Lockscreen/menu").Include(
                "~/Scripts/Examples/Lockscreen-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Login").Include(
                "~/Scripts/Examples/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Login/menu").Include(
                "~/Scripts/Examples/Login-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/P404/menu").Include(
                "~/Scripts/Examples/P404-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/P500/menu").Include(
                "~/Scripts/Examples/P500-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace").Include(
                "~/Scripts/Examples/Pace.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace/menu").Include(
                "~/Scripts/Examples/Pace-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/ProfileEx/menu").Include(
                "~/Scripts/Examples/ProfileEx-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Register").Include(
                "~/Scripts/Examples/Register.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Register/menu").Include(
                "~/Scripts/Examples/Register-menu.js"));
        }

        private static void RegisterMailbox(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox").Include(
                "~/Scripts/Mailbox/Inbox.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox/menu").Include(
                "~/Scripts/Mailbox/Inbox-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose").Include(
                "~/Scripts/Mailbox/Compose.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose/menu").Include(
               "~/Scripts/Mailbox/Compose-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Read/menu").Include(
                "~/Scripts/Mailbox/Read-menu.js"));
        }

        private static void RegisterCalendar(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Calendar").Include(
                "~/Scripts/Calendar/Calendar.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Calendar/menu").Include(
                "~/Scripts/Calendar/Calendar-menu.js"));
        }

        private static void RegisterTables(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Tables/Simple/menu").Include(
                "~/Scripts/Tables/Simple-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Tables/Data").Include(
                "~/Scripts/Tables/Data.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Settings/Data-r").Include(
                "~/Scripts/Settings/Data.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Tables/Data/menu").Include(
                "~/Scripts/Tables/Data-menu.js"));
        }

        private static void RegisterForms(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced-r").Include(
                "~/Scripts/Forms/Advanced.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced/menu-r").Include(
                "~/Scripts/Forms/Advanced-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors-r").Include(
                "~/Scripts/Forms/Editors.js",
                new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors/menu-r").Include(
                "~/Scripts/Forms/Editors-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Plugin/PDF-r").Include(
                "~/Scripts/Plugin/pdfobject.js", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/Scripts/Client/Invoice-r").Include(
                "~/Scripts/Client/invoicestatement.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/Aging-r").Include(
                "~/Scripts/Client/aging.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/Details-r").Include(
                "~/Scripts/Client/details.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/AgingLimit-r").Include(
                "~/Scripts/Client/aginglimit.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/Orders-r").Include(
                "~/Scripts/Client/orders.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/MyOrders-r").Include(
                "~/Scripts/Client/myorders.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Client/MyPrices-r").Include(
                "~/Scripts/Client/myprice.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Price/main-r").Include(
               "~/Scripts/Price/main.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/Supplier/Invoice-r").Include(
                "~/Scripts/Supplier/invoicestatement.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Supplier/Aging-r").Include(
                "~/Scripts/Supplier/aging.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Stock/Main-r").Include(
                "~/Scripts/Stock/main.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Donation/Main-r")
                .Include(
                "~/Scripts/Donation/main.js"
                )
                .Include(
                "~/Scripts/js/bootstrap-datepicker.js"
                )
                );

            bundles.Add(new StyleBundle("~/Content/Stock/css-r").Include(
               "~/Content/css/style.css"));



        }

        private static void RegisterElements(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Elements/Buttons/menu-r").Include(
                "~/Scripts/Elements/Buttons-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/General/menu-r").Include(
                "~/Scripts/Elements/General-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/General-r").Include(
                "~/Styles/Elements/General.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Icons/menu-r").Include(
                "~/Scripts/Elements/Icons-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/Icons-r").Include(
                "~/Styles/Elements/Icons.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Modals/menu-r").Include(
                "~/Scripts/Elements/Modals-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/Modals-r").Include(
                "~/Styles/Elements/Modals.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders-r").Include(
                "~/Scripts/Elements/Sliders.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders/menu-r").Include(
                "~/Scripts/Elements/Sliders-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Timeline/menu-r").Include(
                "~/Scripts/Elements/Timeline-menu.js"));

        }

        private static void RegisterWidgets(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Widgets/menu-r").Include(
                "~/Scripts/Widgets/Widgets-menu.js"));
        }

        private static void RegisterCharts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs-r").Include(
                "~/Scripts/Charts/ChartsJs.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs/menu-r").Include(
                            "~/Scripts/Charts/ChartsJs-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris-r").Include(
                "~/Scripts/Charts/Morris.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris/menu-r").Include(
                "~/Scripts/Charts/Morris-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot-r").Include(
                "~/Scripts/Charts/Flot.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot/menu-r").Include(
                "~/Scripts/Charts/Flot-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline-r").Include(
                "~/Scripts/Charts/Inline.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline/menu-r").Include(
                "~/Scripts/Charts/Inline-menu.js"));
        }

        private static void RegisterHome(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1-r").Include(
                "~/Scripts/Home/DashboardV1.js", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1/menu-r").Include(
                "~/Scripts/Home/DashboardV1-menu.js", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV2/menu-r").Include(
                "~/Scripts/Home/DashboardV2-menu.js"));
        }

        private static void RegisterAccount(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Account/Login-r").Include(
                "~/Scripts/Account/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Account/Register-r").Include(
                "~/Scripts/Account/Register.js"));
        }

        private static void RegisterShared(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/Scripts/Shared/_Layout-r")
                .Include("~/Scripts/Shared/_Layout.js",
                         "~/Scripts/Shared/Common.js")
                );
        }

        private static void RegisterLayout(BundleCollection bundles)
        {
            // bootstrap
            bundles.Add(new ScriptBundle("~/AdminLTE/bootstrap/js-r").Include(
                "~/AdminLTE/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/css-r").Include(
                "~/AdminLTE/bootstrap/css/bootstrap.min.css"));

            // dist
            bundles.Add(new ScriptBundle("~/AdminLTE/dist/js-r").Include(
                "~/AdminLTE/dist/js/app.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/dist/css-r").Include(
                "~/AdminLTE/dist/css/admin-lte.min.css",
                "~/Styles/CustomStyle.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/dist/css/skins-r").Include(
                "~/AdminLTE/dist/css/skins/_all-skins.min.css"));

            // documentation
            bundles.Add(new ScriptBundle("~/AdminLTE/documentation/js-r").Include(
                "~/AdminLTE/documentation/js/docs.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/documentation/css-r").Include(
                "~/AdminLTE/documentation/css/style.css"));

            // plugins | bootstrap-slider
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/bootstrap-slider/js-r").Include(
                                        "~/AdminLTE/plugins/bootstrap-slider/js/bootstrap-slider.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/bootstrap-slider/css-r").Include(
                                        "~/AdminLTE/plugins/bootstrap-slider/css/slider.css"));

            // plugins | bootstrap-wysihtml5
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/bootstrap-wysihtml5/js-r").Include(
                                         "~/AdminLTE/plugins/bootstrap-wysihtml5/js/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/bootstrap-wysihtml5/css-r").Include(
                                        "~/AdminLTE/plugins/bootstrap-wysihtml5/css/bootstrap3-wysihtml5.min.css"));

            // plugins | chartjs
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/chartjs/js-r").Include(
                                         "~/AdminLTE/plugins/chartjs/js/chart.min.js"));

            // plugins | ckeditor
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/ckeditor/js-r").Include(
                                         "~/AdminLTE/plugins/ckeditor/js/ckeditor.js", new CssRewriteUrlTransform()));
            // plugins | ckeditor
            bundles.Add(new ScriptBundle("~/Scripts/ckeditor")
                .Include("~/Scripts/CkeditorCst/js/ckeditor.js", new CssRewriteUrlTransform()));



            // plugins | colorpicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/colorpicker/js-r").Include(
                                         "~/AdminLTE/plugins/colorpicker/js/bootstrap-colorpicker.min.js"));



            bundles.Add(new StyleBundle("~/AdminLTE/plugins/colorpicker/css-r").Include(
                                        "~/AdminLTE/plugins/colorpicker/css/bootstrap-colorpicker.css"));

            // plugins | datatables
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/datatables/js-r").Include(
                                         "~/AdminLTE/plugins/datatables/js/jquery.dataTables.min.js",
                                         "~/AdminLTE/plugins/datatables/js/dataTables.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/datatables/css-r").Include(
                                        "~/AdminLTE/plugins/datatables/css/dataTables.bootstrap.css"));

            // plugins | datepicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/datepicker/js-r").Include(
                                         "~/AdminLTE/plugins/datepicker/js/bootstrap-datepicker.js",
                                         "~/AdminLTE/plugins/datepicker/js/locales/bootstrap-datepicker*"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/datepicker/css-r").Include(
                                        "~/AdminLTE/plugins/datepicker/css/datepicker3.css"));

            // plugins | daterangepicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/daterangepicker/js-r").Include(
                                         "~/AdminLTE/plugins/daterangepicker/js/moment.min.js",
                                         "~/Scripts/js/bootstrap-combobox.js",
                                         "~/AdminLTE/plugins/daterangepicker/js/daterangepicker.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/daterangepicker/css-r").Include(
                                        "~/AdminLTE/plugins/daterangepicker/css/daterangepicker-bs3.css"));

            // plugins | fastclick
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/fastclick/js-r").Include(
                                         "~/AdminLTE/plugins/fastclick/js/fastclick.min.js"));

            // plugins | flot
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/flot/js-r").Include(
                                         "~/AdminLTE/plugins/flot/js/jquery.flot.min.js",
                                         "~/AdminLTE/plugins/flot/js/jquery.flot.resize.min.js",
                                         "~/AdminLTE/plugins/flot/js/jquery.flot.pie.min.js",
                                         "~/AdminLTE/plugins/flot/js/jquery.flot.categories.min.js"));

            // plugins | font-awesome
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/font-awesome/css-r").Include(
                                        "~/AdminLTE/plugins/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // plugins | fullcalendar
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/fullcalendar/js-r").Include(
                                         "~/AdminLTE/plugins/fullcalendar/js/fullcalendar.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/fullcalendar/css-r").Include(
                                        "~/AdminLTE/plugins/fullcalendar/css/fullcalendar.min.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/fullcalendar/css/print-r").Include(
                                        "~/AdminLTE/plugins/fullcalendar/css/print/fullcalendar.print.css"));

            // plugins | icheck
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/icheck/js-r").Include(
                                         "~/AdminLTE/plugins/icheck/js/icheck.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/css-r").Include(
                                        "~/AdminLTE/plugins/icheck/css/all.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/css/flat-r").Include(
                                        "~/AdminLTE/plugins/icheck/css/flat/flat.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/css/sqare/blue-r").Include(
                                        "~/AdminLTE/plugins/icheck/css/sqare/blue.css"));

            // plugins | input-mask
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/input-mask/js-r").Include(
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.js",
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.date.extensions.js",
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.extensions.js"));

            // plugins | ionicons
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/ionicons/css-r").Include(
                                        "~/AdminLTE/plugins/ionicons/css/ionicons.min.css", new CssRewriteUrlTransform()));

            // plugins | ionslider
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/ionslider/js-r").Include(
                                         "~/AdminLTE/plugins/ionslider/js/ion.rangeSlider.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/ionslider/css-r").Include(
                                        "~/AdminLTE/plugins/ionslider/css/ion.rangeSlider.css",
                                        "~/AdminLTE/plugins/ionslider/css/ion.rangeSlider.skinNice.css"));

            // plugins | jquery
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery/js-r").Include(
                                         "~/AdminLTE/plugins/jquery/js/jQuery-2.1.4.min.js", new CssRewriteUrlTransform()));

            // plugins | jquery-validate
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery-validate/js-r").Include(
                                         "~/AdminLTE/plugins/jquery-validate/js/jquery.validate*"));

            // plugins | jquery-ui
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery-ui/js-r").Include(
                                         "~/AdminLTE/plugins/jquery-ui/js/jquery-ui.min.js"));

            // plugins | jvectormap
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jvectormap/js-r").Include(
                                         "~/AdminLTE/plugins/jvectormap/js/jquery-jvectormap-1.2.2.min.js",
                                         "~/AdminLTE/plugins/jvectormap/js/jquery-jvectormap-world-mill-en.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/jvectormap/css-r").Include(
                                        "~/AdminLTE/plugins/jvectormap/css/jquery-jvectormap-1.2.2.css"));

            // plugins | knob
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/knob/js-r").Include(
                                         "~/AdminLTE/plugins/knob/js/jquery.knob.js"));

            // plugins | morris
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/morris/css-r").Include(
                                        "~/AdminLTE/plugins/morris/css/morris.css"));

            // plugins | momentjs
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/momentjs/js-r").Include(
                                         "~/AdminLTE/plugins/momentjs/js/moment.min.js"));

            // plugins | pace
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/pace/js-r").Include(
                                         "~/AdminLTE/plugins/pace/js/pace.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/pace/css-r").Include(
                                        "~/AdminLTE/plugins/pace/css/pace.min.css"));

            // plugins | slimscroll
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/slimscroll/js-r").Include(
                                         "~/AdminLTE/plugins/slimscroll/js/jquery.slimscroll.min.js"));

            // plugins | sparkline
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/sparkline/js-r").Include(
                                         "~/AdminLTE/plugins/sparkline/js/jquery.sparkline.min.js"));

            // plugins | timepicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/timepicker/js-r").Include(
                                         "~/AdminLTE/plugins/timepicker/js/bootstrap-timepicker.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/timepicker/css-r").Include(
                                        "~/AdminLTE/plugins/timepicker/css/bootstrap-timepicker.min.css"));

            // plugins | raphael
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/raphael/js-r").Include(
                                         "~/AdminLTE/plugins/raphael/js/raphael-min.js"));

            // plugins | select2
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/select2/js-r").Include(
                                         "~/AdminLTE/plugins/select2/js/select2.full.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/select2/css-r").Include(
                                        "~/AdminLTE/plugins/select2/css/select2.min.css"));

            // plugins | morris
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/morris/js-r").Include(
                                         "~/AdminLTE/plugins/morris/js/morris.min.js", new CssRewriteUrlTransform()));

        }
    }
}
