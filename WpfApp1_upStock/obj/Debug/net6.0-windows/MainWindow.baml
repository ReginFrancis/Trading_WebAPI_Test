   M S B A M L   `   `   `  ����J  FWpfApp1_upStock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApp1_upStockWpfApp1_upStock  5   	        WpfApp1_upStock.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApp1_upStock     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� -   5        $��Fetch Historical Data��5      $	��200��61   $	��100��6=   6J   �� 5   
   -   5         ��Name$ txtHistoricalData��5      $��1��6    �� TextWrapping$
 Wrap=�6:   " �� VerticalScrollBarVisibility$
 Auto=�6N   �� 5   
   -   5        $��2��5      $��Login with Upstox��6   $��Center=�6U   $��Center=�6r   (  "System.Windows.Controls.WebBrowser  5   
   -   5          Name$ 
webBrowser��5   "   $��4��6   $y�	Collapsed=�66   $��10q�6e   �� 5   
   -   5        $ txtAccessToken��5      $��5��6   $
 Wrap=�67   $
 Auto=�6K   $ �� HorizontalScrollBarVisibility$
 Auto=�6n   $;��5Access Token will appear here after successful login.��5       �� 
IsReadOnly$
 True��6O   $��10q�6a   5      5      