namespace BlackJackWPF
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Data;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Runtime.CompilerServices;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
using System.Windows.Controls;
  using System.Windows.Media;
  using System.Windows.Media.Animation;

  /// <summary>
  /// The Table/Game logic
  /// </summary>
  public class TableVM : INotifyPropertyChanged
  {
    /// <summary>
    /// The list of players playing the game
    /// </summary>
    private ObservableCollection<PersonVM> players;

    /// <summary>
    /// The dealer
    /// </summary>
    private PersonVM dealer;

    /// <summary>
    /// The number of players playing
    /// </summary>
    private int numPlayers;

    /// <summary>
    /// The number of players that are playing
    /// </summary>
    public static int staticNumPlayers;

    /// <summary>
    /// The Deck for this game
    /// </summary>
    private DeckVM deck;

    /// <summary>
    /// Visibility of the new game and quit buttons
    /// </summary>
    private Visibility nextHandButton;

    /// <summary>
    /// Visibility of the PlayerName text boxes and button
    /// </summary>
    private Visibility playerNameTxtBoxes;

    /// <summary>
    /// Visibility of the Number of Players text box and button
    /// </summary>
    private Visibility numPlayersTxtBox;

    /// <summary>
    /// Visibility of the Players Cards and Names
    /// </summary>
    private Visibility playerCards;

    /// <summary>
    /// Visibility of the Start Game button
    /// </summary>
    private Visibility startGameButton;

    /// <summary>
    /// Constructor for TableVm
    /// </summary>
    public TableVM()
    {
      this.deck = new DeckVM();
      this.players = new ObservableCollection<PersonVM>();
      this.dealer = new PersonVM() { PlayerNumber = 0, PlayerName = "Dealer" };
      this.numPlayers = 0;

      this.nextHandButton = Visibility.Hidden;
      this.playerNameTxtBoxes = Visibility.Hidden;
      this.numPlayersTxtBox = Visibility.Visible;
      this.playerCards = Visibility.Hidden;
      this.startGameButton = Visibility.Hidden;

      this.NextHandCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => true,
        ExecuteDelegate = x => this.NextHandCmdExecuted()
      };
      this.QuitGameCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => true,
        ExecuteDelegate = x => this.QuitGameCmdExecuted()
      };
      this.CreatePlayersCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => true,
        ExecuteDelegate = x => this.CreatePlayersCmdExecuted()
      };
      this.LayoutPlayersCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => this.LayoutPlayersCanExecute(),
        ExecuteDelegate = x => this.LayoutPlayersCmdExecuted()
      };
      this.StartGameCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => true,
        ExecuteDelegate = x => this.StartGameCmdExecuted()
      };
      this.StandCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => StandCanExecute(x),
        ExecuteDelegate = x => this.StandCmdExecuted(x)
      };
      this.HitCommand = new CommandHelper()
      {
        CanExecuteDelegate = x => this.HitCanExecute(x),
        ExecuteDelegate = x => this.HitCmdExecuted(x)
      };
    }

    /// <summary>
    /// Creates a new deck
    /// </summary>
    public DeckVM Deck
    {
      get
      {
        return this.deck;
      }
      private set
      {
        this.deck = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and sets the players that and their attributes
    /// </summary>
    public ObservableCollection<PersonVM> Players
    {
      get
      {
        return this.players;
      }
      set
      {
        this.players = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and sets the dealer and his attributes
    /// </summary>
    public PersonVM Dealer
    {
      get
      {
        return this.dealer;
      }
      set
      {
        this.dealer = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and sets the number of players that will be playing determined by the user
    /// </summary>
    public int NumPlayers
    {
      get
      {
        return this.numPlayers;
      }
      set
      {
        this.numPlayers = value;
        this.OnNotify();
        staticNumPlayers = value;
      }
    }

    /// <summary>
    /// The visibility of the Start Game button
    /// </summary>
    public Visibility StartGameButton
    {
      get
      {
        return this.startGameButton;
      }
      set
      {
        this.startGameButton = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the Players info
    /// </summary>
    public Visibility PlayerCards
    {
      get
      {
        return this.playerCards;
      }
      set
      {
        this.playerCards = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the Number of Player text boxes and buttons
    /// </summary>
    public Visibility NumPlayersTxtBox
    {
      get
      {
        return this.numPlayersTxtBox;
      }
      set
      {
        this.numPlayersTxtBox = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the PlayerName text boxes
    /// </summary>
    public Visibility PlayerNameTxtBoxes
    {
      get
      {
        return this.playerNameTxtBoxes;
      }
      set
      {
        this.playerNameTxtBoxes = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the NewGame and Quit buttons
    /// </summary>
    public Visibility NextHandButton
    {
      get
      {
        return this.nextHandButton;
      }
      set
      {
        this.nextHandButton = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// A command for the new game button
    /// </summary>
    public CommandHelper NextHandCommand { get; private set; }

    /// <summary>
    /// A command for the quit button
    /// </summary>
    public CommandHelper QuitGameCommand { get; private set; }

    /// <summary>
    /// A command for the Next button that creates the
    /// number of players determined by the user
    /// </summary>
    public CommandHelper CreatePlayersCommand { get; set; }

    /// <summary>
    /// A command for the Layout Players Button
    /// </summary>
    public CommandHelper LayoutPlayersCommand { get; set; }

    /// <summary>
    /// A command for the Start Game Button
    /// </summary>
    public CommandHelper StartGameCommand { get; set; }

    /// <summary>
    /// A command for the Stand Button
    /// </summary>
    public CommandHelper StandCommand { get; set; }

    /// <summary>
    /// A command for the Hit Button
    /// </summary>
    public CommandHelper HitCommand { get; set; }

    /// <summary>
    /// Verifies the object being passed is not Null and is a Person
    /// </summary>
    /// <param name="parameter">A person </param>
    /// <returns> Returns true </returns>
    private bool HitCanExecute(object parameter)
    {
      return parameter != null && parameter is PersonVM;
    }

    /// <summary>
    /// Adds a card to the current players hand and checks to see if
    /// they have busted as a result. If they have it sets the next
    /// players hit and stand buttons and hand to visible
    /// </summary>
    /// <param name="parameter">The current player </param>
    private void HitCmdExecuted(object parameter)
    {
      PersonVM person = (PersonVM)parameter;
      person.DealCard(this.deck);
      if (person.Bust == true)
      {
        person.HitAndStand = Visibility.Hidden;
        person.BustTextBlock = Visibility.Visible;
        person.HandIsVisible = false;
        int playerNum = this.players.IndexOf(person);
        int index;
        for (index = playerNum + 1; index <= players.Count - 1; index++)
        {
          if (players[index].OutOfMoney == false)
          {
            players[index].HandIsVisible = true;
            players[index].HitAndStand = Visibility.Visible;
            foreach (CardVM card in players[index].CardsInHand)
            {
              card.FaceUp = true;
            }
            break;
          }
        }
        if (index > players.Count - 1)
        {
          dealer.HandIsVisible = true;
          foreach (CardVM card in dealer.CardsInHand)
          {
            card.FaceUp = true;
          }

          bool playing = true;
          while (playing)
          {
            if (dealer.Bust == false)
            {
              if (dealer.Score2 < 17)
              {
                dealer.DealCard(deck);
              }
              else if (dealer.Score2 <= 21)
              {
                dealer.HandIsVisible = false;
                playing = false;
              }
              else if (dealer.Score1 < 17)
              {
                dealer.DealCard(deck);
              }
              else
              {
                dealer.HandIsVisible = false;
                playing = false;
              }
            }
            else
            {
              dealer.BustTextBlock = Visibility.Visible;
              playing = false;
            }
          }
          foreach (PersonVM player in players)
          {
            if (player.Bust == true)
            {
              player.Money = player.Money - player.BetText;
            }
            else
            {
              if (dealer.Bust == true)
              {
                if (player.Score1 == 21 || player.Score2 == 21)
                {
                  player.Money = player.Money + (player.BetText * 1.5);
                }
                else
                {
                  player.Money = player.Money + player.BetText;
                }
              }
              else
              {
                if (player.Score2 <= 21)
                {
                  if (dealer.Score2 <= 21)
                  {
                    if (player.Score2 < dealer.Score2)
                    {
                      player.Money = player.Money - player.BetText;
                    }
                    else if (player.Score2 > dealer.Score2)
                    {
                      if (player.Score2 == 21)
                      {
                        player.Money = player.Money + (player.BetText * 1.5);
                      }
                      else
                      {
                        player.Money = player.Money + player.BetText;
                      }
                    }
                  }
                  else
                  {
                    if (player.Score2 < dealer.Score1)
                    {
                      player.Money = player.Money - player.BetText;
                    }
                    else if (player.Score2 > dealer.Score1)
                    {
                      if (player.Score2 == 21)
                      {
                        player.Money = player.Money + (player.BetText * 1.5);
                      }
                      else
                      {
                        player.Money = player.Money + player.BetText;
                      }
                    }
                  }
                }
                else
                {
                  if (dealer.Score2 <= 21)
                  {
                    if (player.Score1 < dealer.Score2)
                    {
                      player.Money = player.Money - player.BetText;
                    }
                    else if (player.Score1 > dealer.Score2)
                    {
                      if (player.Score1 == 21)
                      {
                        player.Money = player.Money + (player.BetText * 1.5);
                      }
                      else
                      {
                        player.Money = player.Money + player.BetText;
                      }
                    }
                  }
                  else
                  {
                    if (player.Score1 < dealer.Score1)
                    {
                      player.Money = player.Money - player.BetText;
                    }
                    else if (player.Score1 > dealer.Score2)
                    {
                      if (player.Score1 == 21)
                      {
                        player.Money = player.Money + (player.BetText * 1.5);
                      }
                      else
                      {
                        player.Money = player.Money + player.BetText;
                      }
                    }
                  }
                }
              }
            }
            if (player.Money <= 0)
            {
              if (player.OutOfMoney == false)
              {
                string NoMoney = player.PlayerName + " is out of money and owe's " + (player.Money * -1) + ".";
                MessageBox.Show(NoMoney);
                player.OutOfMoney = true;
                player.BetOnTable = Visibility.Hidden;
                player.NoMoney = Visibility.Visible;
              }
              else
              {

              }
            }
          }
        }
      }
      
      this.NextHandButton = Visibility.Visible;
    }


    /// <summary>
    /// Creates a New Game when the NewGame button is pressed
    /// </summary>
    private void NextHandCmdExecuted()
    {
      foreach (PersonVM player in this.players)
      {
        player.CardsInHand.Clear();
        player.Bust = false;
        player.HandIsVisible = false;
        player.BustTextBlock = Visibility.Hidden;
        player.BetOnTable = Visibility.Hidden;
        player.BetVisibility = Visibility.Visible;
        player.BetText = 0;
      }
      this.dealer.CardsInHand.Clear();
      this.dealer.Bust = false;
      this.dealer.HandIsVisible = false;
      this.dealer.BustTextBlock = Visibility.Hidden;

      this.deck.ClearUsedCards();

      for (int i = 0; i < 2; i++)
      {
        foreach (PersonVM person in this.players)
        {
          person.DealCard(deck);
        }

        this.dealer.DealCard(deck);
      }

      this.NextHandButton = Visibility.Hidden;
      this.StartGameButton = Visibility.Visible;
    }

    /// <summary>
    /// Closes the window when the Quit button is pressed
    /// </summary>
    private void QuitGameCmdExecuted()
    {
      Application.Current.Shutdown();
    }

    /// <summary>
    /// Creates the number of players determined by the user
    /// </summary>
    private void CreatePlayersCmdExecuted()
    {
      string message;
      if (numPlayers == 0 || numPlayers < 1 || numPlayers > 5)
      {
        message = "Invalid Input. Please enter a number between 1 and 5.";
        MessageBox.Show(message);
      }
      else
      {
        for (int i = 0; i < numPlayers; i++)
        {
          PersonVM person = new PersonVM() { PlayerNumber = i + 1 };
          players.Add(person);
        }

        this.NumPlayersTxtBox = Visibility.Hidden;
        this.PlayerNameTxtBoxes = Visibility.Visible;
      }
    }

    /// <summary>
    /// Lays the players cards, name and show cards button out on the table
    /// </summary>
    public void LayoutPlayersCmdExecuted()
    {
      this.deck.CreateDeck();
      for (int i = 0; i < 2; i++)
      {
        foreach (PersonVM person in players)
        {
          person.DealCard(deck);
        }
        dealer.DealCard(deck);
      }
      this.PlayerNameTxtBoxes = Visibility.Hidden;
      foreach (PersonVM player in players)
      {
        player.BetVisibility = Visibility.Visible;
      }
      this.PlayerCards = Visibility.Visible;
      this.StartGameButton = Visibility.Visible;
    }

    /// <summary>
    /// Checks to see if all the names have been set. If they have not
    /// the game can not be started until they have
    /// </summary>
    /// <returns>Returns whether or not all the names have been set </returns>
    public bool LayoutPlayersCanExecute()
    {
      bool AllNamesSet = players.ToList().Any(p => string.IsNullOrEmpty(p.PlayerName));
      return !AllNamesSet;
    }

    /// <summary>
    /// Set's all the cards for the first players hand to face up and makes
    /// player 1's hit and stand buttons visibile to use
    /// </summary>
    public void StartGameCmdExecuted()
    {
      foreach (PersonVM person in players)
      {
        if (person.OutOfMoney == false)
        {
          person.Bet = person.BetText;
        }
        else
        {
          person.BetText = 0;
          person.Bet = 5;
        }
      }
      if (players.ToList().Any(p => p.Bet < 5))
      {
        string message = "Each player must place a bet of at least 10 to continue.";
        MessageBox.Show(message);
      }
      else
      {
        foreach (PersonVM player in players)
        {
          player.BetVisibility = Visibility.Hidden;
          player.BetOnTable = Visibility.Visible;
        }
        int index;
        for (index = 0; index <= players.Count - 1; index++)
        {
          if (players[index].OutOfMoney == false)
          {
            players[index].HandIsVisible = true;
            players[index].HitAndStand = Visibility.Visible;
            foreach (CardVM card in players[index].CardsInHand)
            {
              card.FaceUp = true;
            }
            break;
          }
        }
        this.StartGameButton = Visibility.Hidden;
      }

    }

    /// <summary>
    /// Determines whether or not the Stand command can execute
    /// </summary>
    /// <param name="parameter">The player invoking this command </param>
    /// <returns>Returns if the object is in fact a person vm. </returns>
    public bool StandCanExecute(object parameter)
    {
      return parameter != null && parameter is PersonVM;
    }

    /// <summary>
    /// Ends the players turn and starts the next players turn if there
    /// is another player
    /// </summary>
    public void StandCmdExecuted(object parameter)
    {
      PersonVM person = (PersonVM)parameter;

      person.HandIsVisible = false;
      person.HitAndStand = Visibility.Hidden;
      int playerNum = this.players.IndexOf(person);
      int index;
      for (index = playerNum + 1; index <= players.Count - 1; index++)
      {
        if (players[index].OutOfMoney == false)
        {
          players[index].HandIsVisible = true;
          players[index].HitAndStand = Visibility.Visible;
          foreach (CardVM card in players[index].CardsInHand)
          {
            card.FaceUp = true;
          }
          break;
        }
      }
      if (index > players.Count - 1)
      {
        dealer.HandIsVisible = true;
        foreach (CardVM card in dealer.CardsInHand)
        {
          card.FaceUp = true;
        }

        bool playing = true;
        while (playing)
        {
          if (dealer.Bust == false)
          {
            if (dealer.Score2 < 17)
            {
              dealer.DealCard(deck);
            }
            else if (dealer.Score2 <= 21)
            {
              dealer.HandIsVisible = false;
              playing = false;
            }
            else if (dealer.Score1 < 17)
            {
              dealer.DealCard(deck);
            }
            else
            {
              dealer.HandIsVisible = false;
              playing = false;
            }
          }
          else
          {
            dealer.BustTextBlock = Visibility.Visible;
            playing = false;
          }
        }
        foreach (PersonVM player in players)
        {
          if (player.Bust == true)
          {
            player.Money = player.Money - player.BetText;
          }
          else
          {
            if (dealer.Bust == true)
            {
              if (player.Score1 == 21 || player.Score2 == 21)
              {
                player.Money = player.Money + (player.BetText * 1.5);
              }
              else
              {
                player.Money = player.Money + player.BetText;
              }
            }
            else
            {
              if (player.Score2 <= 21)
              {
                if (dealer.Score2 <= 21)
                {
                  if (player.Score2 < dealer.Score2)
                  {
                    player.Money = player.Money - player.BetText;
                  }
                  else if (player.Score2 > dealer.Score2)
                  {
                    if (player.Score2 == 21)
                    {
                      player.Money = player.Money + (player.BetText * 1.5);
                    }
                    else
                    {
                      player.Money = player.Money + player.BetText;
                    }
                  }
                }
                else
                {
                  if (player.Score2 < dealer.Score1)
                  {
                    player.Money = player.Money - player.BetText;
                  }
                  else if (player.Score2 > dealer.Score1)
                  {
                    if (player.Score2 == 21)
                    {
                      player.Money = player.Money + (player.BetText * 1.5);
                    }
                    else
                    {
                      player.Money = player.Money + player.BetText;
                    }
                  }
                }
              }
              else
              {
                if (dealer.Score2 <= 21)
                {
                  if (player.Score1 < dealer.Score2)
                  {
                    player.Money = player.Money - player.BetText;
                  }
                  else if (player.Score1 > dealer.Score2)
                  {
                    if (player.Score1 == 21)
                    {
                      player.Money = player.Money + (player.BetText * 1.5);
                    }
                    else
                    {
                      player.Money = player.Money + player.BetText;
                    }
                  }
                }
                else
                {
                  if (player.Score1 < dealer.Score1)
                  {
                    player.Money = player.Money - player.BetText;
                  }
                  else if (player.Score1 > dealer.Score2)
                  {
                    if (player.Score1 == 21)
                    {
                      player.Money = player.Money + (player.BetText * 1.5);
                    }
                    else
                    {
                      player.Money = player.Money + player.BetText;
                    }
                  }
                }
              }
            }
          }
          if (player.Money <= 0)
          {
            if (player.OutOfMoney == false)
            {
              string NoMoney = player.PlayerName + " is out of money and owe's " + (player.Money * -1) + ".";
              MessageBox.Show(NoMoney);
              player.OutOfMoney = true;
              player.BetOnTable = Visibility.Hidden;
              player.NoMoney = Visibility.Visible;
            }
            else
            {

            }
          }
        }
        this.NextHandButton = Visibility.Visible;
      }
    }


    /// <summary>
    /// Refreshes view if property changed is not null
    /// </summary>
    /// <param name="PropertyName">Name of the property that changed </param>
    private void OnNotify([CallerMemberName]string PropertyName = null)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
      }
    }

    /// <summary>
    /// An event that recognizes if a property has changed
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
  }
}