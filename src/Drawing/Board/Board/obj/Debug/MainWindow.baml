   M S B A M L   `   `   `  ����@  <Board, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:BoardBoard  5   	       Board.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Board     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��1* �6   �� 5      $��9* �6    � RowDefinitions 5   
   �� 5      $��1* �6   .+�5   
   �� $��0��6   $��0��6%   $��Stretch=�5      $��Stretch=�63   $��0,0,0,0q�5      
�������.+�5      A� -   5         A�Name$ buttonBlack��5      
���   �$��50��5      $��50��6!   $��10, 10, 10, 10q�5      5      A� 5      -   5        $ 	buttonRed��5      
���  ��$��50��5      $��50��6!   $��10, 10, 10, 10q�5      5       A� 5!      -   5        $ 
buttonBlue��5!      
����  �$��50��5#      $��50��6!   $��10, 10, 10, 10q�5$      5%      A� 5&      -   5        $ buttonGreen��5&      
��� � �$��50��5(      $��50��6!   $��10, 10, 10, 10q�5)      5*      A� 5+      -   5        $ buttonWhite��5+      
�������$��50��5-      $��50��6!   $��10, 10, 10, 10q�5.      5/      50      �� 52   
   -   5         ��Name$ canvasBoard��52      $��1��53      $��0��6!   
�������59      5;      5<      