using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace BlackJackWPF
{
  /// <summary>
  /// Assigns each suit a number
  /// </summary>
  public enum Suit
  {
    clubs = 1,
    diamonds,
    spades,
    hearts
  }
  

  /// <summary>
  /// A class that contains attributes for a card object 
  /// </summary>
  public class CardVM : INotifyPropertyChanged
  {
    /// <summary>
    /// A variable that holds the cards value
    /// </summary>
    private string cardValue;

    /// <summary>
    /// A variable that holds the cards suit
    /// </summary>
    private Suit suit;
    
    /// <summary>
    /// The margin that will seperate the cards in the players hand
    /// </summary>
    private Thickness cardoffset;

    /// <summary>
    /// The amount the card is rotated
    /// </summary>
    private Transform cardtransform;

    /// <summary>
    /// A bool that signifies if the card is face up or not
    /// </summary>
    private bool faceUp;

    /// <summary>
    /// The card value image source rectangle
    /// </summary>
    private Int32Rect sourceRect;

    /// <summary>
    /// X part of start point
    /// </summary>
    private int startx;

    /// <summary>
    /// Y part of start point
    /// </summary>
    private int starty;

    public CardVM()
    {
    }
    
    /// <summary>
    /// A constructor that creates a card
    /// </summary>
    /// <param name="cardValue"> Value of the card</param>
    /// <param name="suit"> Suit of the card</param>
    public CardVM(string cardValue, Suit suit)
    {
      this.cardValue = cardValue;
      this.suit = suit;
      this.FaceUp = false;
      this.startx = 0;
      this.starty = 0;
    }

    /// <summary>
    /// Gets and sets the amount the card is rotated
    /// </summary>
    public Transform CardTransform
    {
      get
      {
        return this.cardtransform;
      }
      set
      {
        this.cardtransform = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and sets the margin that will seperate the cards in the players hand
    /// </summary>
    public Thickness CardOffset
    {
      get
      {
        return this.cardoffset;
      }
      set
      {
        this.cardoffset = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and sets the cards image source
    /// </summary>
    public Int32Rect CardSource
    {
      get
      {
        return this.sourceRect;
      }
      set
      {
        this.sourceRect = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets and returns whether the card is face up or not
    /// </summary>
    /// <returns> Face Up bool</returns>
    public bool FaceUp
    {
      get
      {
        return faceUp;
      }
      set
      {
        this.faceUp = value;
        this.OnNotify();
      }
    }
   
    /// <summary>
    /// Gets and returns the cards value
    /// </summary>
    /// <returns> The cards value</returns>
    public string Get_cardValue()
    {
      return this.cardValue;
    }

    /// <summary>
    /// Gets and returns the card's suit
    /// </summary>
    /// <returns> The cards suit</returns>
    //public string Get_cardSuit()
    //{
    //  if (this.suit == Suit.clubs)
    //  {
    //    return "Clubs";
    //  }
    //  else if (this.suit == Suit.diamonds)
    //  {
    //    return "Diamonds";
    //  }
    //  else if (this.suit == Suit.hearts)
    //  {
    //    return "Hearts";
    //  }
    //  else
    //  {
    //    return "Spades";
    //  }
    //}

    /// <summary>
    /// Sets the card Image source based on the value and suit
    /// </summary>
    /// <returns> The source rectangle for that card</returns>
    public Int32Rect SourceRect()
    {
      if (cardValue == "Ace")
      {
        this.startx = 0;
      }
      else if (cardValue == "Jack")
      {
        this.startx = Convert.ToInt32(Math.Round(181.5 * 10));
      }
      else if (cardValue == "Queen")
      {
        this.startx = Convert.ToInt32(Math.Round(181.5 * 11));
      }
      else if (cardValue == "King")
      {
        this.startx = Convert.ToInt32(Math.Round(181.5 * 12));
      }
      else
      {
        int tempvalue = int.Parse(cardValue);
        this.startx = Convert.ToInt32(Math.Round(181.5 * (tempvalue - 1)));
      }

      if (suit == Suit.hearts)
      {
        this.starty = Convert.ToInt32(Math.Round(455.5 + (251.5 * 0)));
      }
      else if (suit == Suit.spades)
      {
        this.starty = Convert.ToInt32(Math.Round(455.5 + (251.5 * 1)));
      }
      else if (suit == Suit.diamonds)
      {
        this.starty = Convert.ToInt32(Math.Round(455.5 + (251.5 * 2)));
      }
      else
      {
        this.starty = Convert.ToInt32(Math.Round(455.5 + (251.5 * 3)));
      }

      return this.sourceRect = new Int32Rect(startx, starty, 181, 251);
    }

    /// <summary>
    /// Refreshes view if property changed is not null
    /// </summary>
    /// <param name="PropertyName"> Name of the property that changed</param>
    private void OnNotify([CallerMemberName]string PropertyName = null)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
