using System;
using System.Collections.Generic;
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
        private List<ListView> players = new List<ListView>();
        
        public MainWindow()
        {
            InitializeComponent();
            viewModel.StartGame(2, new[] { "First Person", "Second Person" });
            ObserveMoveState();
            
            CurrentStackOfCards.ItemsSource = viewModel.CurrentStack;
            TrumpCard.Source = new BitmapImage(new Uri(viewModel.TrumpCard.imageName, UriKind.Relative)); 
            FirstPlayerCards.ItemsSource = viewModel.Players[0].cards;
            SecondPlayerCards.ItemsSource = viewModel.Players[1].cards;
            players.Add(FirstPlayerCards);
            players.Add(SecondPlayerCards);
            
            foreach (var listView in players)
            {
                listView.ChangeVisible();
            }
            players[viewModel.FirstMove].ChangeVisible();
            TakeButton.IsEnabled = false;

            viewModel.MoveIfBotRound();
        }

        private void ObserveMoveState()
        {
            viewModel.viewMoveState.Observe((currentMoveState) =>
            {
                if (currentMoveState is ChangeCurrentStep move)
                {
                    var firstPosition = move.FirstPosition; 
                    var secondPosition =  move.SecondPosition; 
                    
                    players[firstPosition].ChangeVisible();
                    players[secondPosition].ChangeVisible();

                    if (viewModel.MoveIsAssualt())
                    {
                        TakeButton.IsEnabled = false;
                        PassButton.IsEnabled = true;
                    }
                    else
                    {
                        TakeButton.IsEnabled = true;
                        PassButton.IsEnabled = false;
                    }
                 
                }

                if (currentMoveState is InabilityMove)
                {
                    MessageBox.Show("Такой картой нельзя сходить");
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