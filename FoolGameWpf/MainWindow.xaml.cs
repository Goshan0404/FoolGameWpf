using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FoolGame;

namespace FoolGameWpf
{
    public partial class MainWindow
    {
        private GameViewModel viewModel = new GameViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            SecondPlayerCards.Visibility = Visibility.Hidden;
            
            ObserveMoveState();
            viewModel.StartGame(2, new[] { "Lox", "One More Lox" });
            
            TrumpCard.Source = new BitmapImage(new Uri(viewModel.TrumpCard.imageName, UriKind.Relative));
            FirstPlayerCards.ItemsSource = viewModel.Players[0].cards;
            SecondPlayerCards.ItemsSource = viewModel.Players[1].cards;
            CurrentStackOfCards.ItemsSource = viewModel.CurrentStack;
        }

        private void ObserveMoveState()
        {
            viewModel.currentMoveStates.Observe((currentMoveState) =>
            {
                if (currentMoveState is ChangeMove)
                {
                    FirstPlayerCards.ChangeVisible();
                    SecondPlayerCards.ChangeVisible();
                }
                
                if (currentMoveState is InabilityMove)
                {
                    MessageBox.Show("Такой картой нельзя сходить");
                }

                if (currentMoveState is AssaultMove)
                {
                    TakeButton.IsEnabled = false;
                    PassButton.IsEnabled = true;
                }

                if (currentMoveState is DefenseMove)
                {
                    PassButton.IsEnabled = false;
                    TakeButton.IsEnabled = true;
                }
            });
        }

        void OnCardClicked(object sender, MouseButtonEventArgs e)
        {
            Card card = (sender as Image)?.DataContext as Card;


            viewModel.Move(card);
        }

        void OnPassButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            viewModel.Move();
        }

        void OnTakeButtonClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            viewModel.Move();
        }
    }

    static class ListViewExtension
    {
        public  static void ChangeVisible(this ListView listView)
        {
            if (listView.Visibility == Visibility.Visible)
                listView.Visibility = Visibility.Hidden;
            else
                listView.Visibility = Visibility.Visible;
        }
    }
}