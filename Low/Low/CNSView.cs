using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Low
{
  public partial class CNSView : Control
  {
    public CNSView()
    {
      InitializeComponent();
    }

    public CNS MyCNS { get; set; }
    
    protected override void OnPaint(PaintEventArgs pe)
    {
      /*Если какой-то предикт не совпадает с сенсорами
        работать на его совпадение.
       * Если памяти эффектора нет - работать на память эффектора.
       * Если целевой сенсор падает - работать на поднятие целевого сенсора.*/

      int lineWidth_ = 3;
      SolidBrush sb = new SolidBrush(Color.Black);
      if ((Width - lineWidth_ * 2 <= 0) || (Height - lineWidth_ * 2 <= 0))
      {
        pe.Graphics.FillRectangle(sb, 0, 0, Width, Height);
        base.OnPaint(pe);
        return;
      }

      SolidBrush rsb = new SolidBrush(Color.Red);

      pe.Graphics.FillRectangle(sb, 0, 0, Width, lineWidth_);
      pe.Graphics.FillRectangle(sb, Width - lineWidth_, 0, Width, Height);
      pe.Graphics.FillRectangle(sb, 0, Height - lineWidth_, Width, Height);
      pe.Graphics.FillRectangle(sb, 0, 0, lineWidth_, Height);

      base.OnPaint(pe);
    }    
  }
}
