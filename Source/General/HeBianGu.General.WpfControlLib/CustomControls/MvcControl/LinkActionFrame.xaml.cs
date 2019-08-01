﻿using HeBianGu.Base.WpfBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HeBianGu.General.WpfControlLib
{
    /// <summary> 自定义导航框架 </summary>

    public class LinkActionFrame : ContentControl
    {
        static LinkActionFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkActionFrame), new FrameworkPropertyMetadata(typeof(LinkActionFrame)));
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public ILinkActionBase LinkAction
        {
            get { return (ILinkActionBase)GetValue(LinkActionProperty); }
            set { SetValue(LinkActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkActionProperty =
            DependencyProperty.Register("LinkAction", typeof(ILinkActionBase), typeof(LinkActionFrame), new PropertyMetadata(default(LinkAction), (d, e) =>
              {
                  LinkActionFrame control = d as LinkActionFrame;

                  if (control == null) return;

                  if (e.NewValue is ILinkActionBase config)
                  {
                      control.RefreshLinkAction(config);
                  }

              }));

        Random random = new Random();

        async Task RefreshLinkAction(ILinkActionBase linkActionBase)
        {
            try
            {
                if (this.UseRandomWipe)
                {
                    this.CurrentWipe = this.RandomWipes[random.Next(this.RandomWipes.Count)];
                }
                else
                {
                    if (linkActionBase is LinkAction linkAction)
                    {
                        this.CurrentWipe = linkAction.TransitionWipe ?? new CircleWipe();
                    }
                }

                var result = await Task.Run(() =>
                {
                    return linkActionBase?.ActionResult();
                });


                this.Dispatcher.Invoke(() =>
                {
                    this.Content = result?.View;
                });
            }
            catch (Exception ex)
            {
                this.Content = ex;
            }
        }

        //async Task RefreshLinkAction(ILinkActionBase linkActionBase)
        //{
        //    try
        //    {
        //        if (this.UseRandomWipe)
        //        {
        //            this.CurrentWipe = this.RandomWipes[random.Next(this.RandomWipes.Count)];
        //        }
        //        else
        //        {
        //            if (linkActionBase is LinkAction linkAction)
        //            {
        //                this.CurrentWipe = linkAction.TransitionWipe ?? new CircleWipe();
        //            }
        //        }

        //        await Task.Run(async () =>
        //        {
        //            var result = await linkActionBase?.ActionResult();

        //            this.Dispatcher.Invoke(() =>
        //            {
        //                this.Content = result?.View;
        //            });

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Content = ex;
        //    }
        //}

        public ITransitionWipe CurrentWipe
        {
            get { return (ITransitionWipe)GetValue(CurrentWipeProperty); }
            set { SetValue(CurrentWipeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentWipeProperty =
            DependencyProperty.Register("CurrentWipe", typeof(ITransitionWipe), typeof(LinkActionFrame), new PropertyMetadata(new CircleWipe(), (d, e) =>
             {
                 LinkActionFrame control = d as LinkActionFrame;

                 if (control == null) return;

                 ITransitionWipe config = e.NewValue as ITransitionWipe;

             }));


        public ObservableCollection<ITransitionWipe> RandomWipes
        {
            get { return (ObservableCollection<ITransitionWipe>)GetValue(RandomWipesProperty); }
            set { SetValue(RandomWipesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RandomWipesProperty =
            DependencyProperty.Register("RandomWipes", typeof(ObservableCollection<ITransitionWipe>), typeof(LinkActionFrame), new PropertyMetadata(new ObservableCollection<ITransitionWipe>()
            {
                 new CircleWipe(),
                 new SlideWipe(){ Direction=SlideDirection.Left},
                 new SlideWipe(){ Direction=SlideDirection.Right},
                 new SlideWipe(){ Direction=SlideDirection.Down},
                 new SlideWipe(){ Direction=SlideDirection.Up},
                 new SlideOutWipe(),
                 new FadeWipe()
             }));


        public bool UseRandomWipe
        {
            get { return (bool)GetValue(UseRandomWipeProperty); }
            set { SetValue(UseRandomWipeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseRandomWipeProperty =
            DependencyProperty.Register("UseRandomWipe", typeof(bool), typeof(TransitionEffectBase), new PropertyMetadata(true));


    }

}