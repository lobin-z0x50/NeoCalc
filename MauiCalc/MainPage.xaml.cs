﻿namespace MauiCalc;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    void MauiCalc_Loaded(System.Object sender, System.EventArgs e)
    {
		(this.BindingContext as MainPageVM)?.OnLoad(sender, e);
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        (this.BindingContext as MainPageVM)?.OnButtonClick(sender, e);
    }
}


