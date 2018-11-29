using System;
using System.Windows.Input;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace TestPopup
{
	public class ColorPalettePopup : PopupPage
    {
        private Command _cancelCommand = null;
        public ICommand CancelCommand {
            get {
                _cancelCommand = _cancelCommand ?? new Command(DoCancelCommand);
                return (_cancelCommand);
            }
        }
        private Command _dockCommand = null;
        public ICommand DockCommand {
            get {
                _dockCommand = _dockCommand ?? new Command(DoDockCommand);
                return (_dockCommand);
            }
        }

        private IPopupNavigation _pn = null;
        private int _numPaletteRows;
        private int _numPaletteCols;

        public ColorPalettePopup(IPopupNavigation pn, bool isHorizontal)
        {
            _pn = pn;
            if (isHorizontal) {
                _numPaletteRows = 1;
                _numPaletteCols = 7;
            } else {
                _numPaletteRows = 7;
                _numPaletteCols = 1;
            }
            BindingContext = this;
            Color[] palette = new Color[] {
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Indigo,
                Color.Violet
            };
            BoxView titleBackground = new BoxView {
                HeightRequest = 0,
                WidthRequest = 0,
                Color = ColorPalettePopupStyle.TITLE_BACKGROUND_COLOR
            };
            Label titleL = new Label {
                TextColor = ColorPalettePopupStyle.TITLE_TEXT_COLOR,
                Margin = new Thickness(0, ColorPalettePopupStyle.TITLE_MARGIN),
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = ColorPalettePopupStyle.TITLE_FONT_SIZE,
                Text = "Color Palette"
            };
            Grid paletteG = new Grid {
                RowSpacing = 0,
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White,
                Padding = new Thickness(ColorPalettePopupStyle.DIALOG_MARGIN)
            };
            for (int c = 0; c < _numPaletteCols; c++)
                paletteG.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            for (int r = 0; r < _numPaletteRows; r++)
                paletteG.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            int paletteIndex = 0;
            for (int r = 0; r < _numPaletteRows; r++) {
                for (int c = 0; c < _numPaletteCols; c++) {
                    if (paletteIndex >= palette.Length)
                        break;
                    Button colorB = new Button {
                        WidthRequest = ColorPalettePopupStyle.COLOR_DIM,
                        HeightRequest = ColorPalettePopupStyle.COLOR_DIM,
                        BackgroundColor = palette[paletteIndex],
                        CornerRadius = 0
                    };
                    colorB.Clicked += HandleColorButtonClicked;
                    paletteG.Children.Add(colorB, c, r);
                    paletteIndex++;
                }
            }
            Button cancelB = new Button {
                Text = "Cancel",
                BackgroundColor = Color.Transparent,
                //Padding = new Thickness(ColorPalettePopupStyle.IMAGE_BUTTON_PADDING)
            };
            Button dockB = new Button {
                Text = "Dock",
                BackgroundColor = Color.Transparent,
                //Padding = new Thickness(ColorPalettePopupStyle.IMAGE_BUTTON_PADDING)
            };
            Grid mainG = new Grid {
                BackgroundColor = Color.White,
                RowSpacing = 0,
                ColumnSpacing = 0,
                //Help layout engine with size of grid
                //WidthRequest = ColorPalettePopupStyle.COLOR_DIM * NUM_PALETTE_COLS + ColorPalettePopupStyle.DIALOG_MARGIN * 6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };
            mainG.Children.Add(titleBackground, 0, 0);
            Grid.SetColumnSpan(titleBackground, 2);
            mainG.Children.Add(titleL, 0, 0);
            Grid.SetColumnSpan(titleL, 2);
            mainG.Children.Add(paletteG, 0, 1);
            Grid.SetColumnSpan(paletteG, 2);
            mainG.Children.Add(cancelB, 0, 2);
            mainG.Children.Add(dockB, 1, 2);
            Content = mainG;
            //Bindings
            cancelB.SetBinding(Button.CommandProperty, nameof(CancelCommand));
            dockB.SetBinding(Button.CommandProperty, nameof(DockCommand));
        }

        private async void HandleColorButtonClicked(object sender, EventArgs e)
        {
            await _pn.PopAsync();
        }

        private async void DoCancelCommand()
        {
            await _pn.PopAsync();
        }

        private async void DoDockCommand()
        {
            await _pn.PopAsync();
        }
    }

    public class ColorPalettePopupStyle
    {
        public static Color TITLE_BACKGROUND_COLOR = Color.FromRgb(242, 242, 242);
        public static Color TITLE_TEXT_COLOR = Color.Black;
        public const int TITLE_MARGIN = 5;
        public const int TITLE_FONT_SIZE = 14;
        public const int DIALOG_MARGIN = 10;
        public static Color MESSAGE_TEXT_COLOR = Color.FromRgb(50, 50, 50);
        public const int MESSAGE_FONT_SIZE = 12;
        public static Color VALUE_TEXT_COLOR = Color.Black;
        public const int VALUE_FONT_SIZE = 14;
        public const int COLOR_DIM = 50;
        public const int IMAGE_BUTTON_PADDING = 10;
    }
}
