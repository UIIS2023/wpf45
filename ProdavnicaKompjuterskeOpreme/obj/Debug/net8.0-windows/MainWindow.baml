   M S B A M L   `   `   `  ����W  SProdavnicaKompjuterskeOpreme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:ProdavnicaKompjuterskeOpremeProdavnicaKompjuterskeOpreme  5   	   -    'ProdavnicaKompjuterskeOpreme.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:ProdavnicaKompjuterskeOpreme     5   	   -   5          � Title$  Computer store��5   	   $	��600��6    $	��950��6-    � WindowStartupLocation$ CenterScreen=�5	   	    � Icon$ images\logo.png��5
   	    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	   
������  � AllowsTransparency$
 True��5   	    � 	Resources 5         NoMouseOverButtonTemplate �� 
TargetType �� Triggers	 P� Property
 P� Value �� Value �� Property �� 
TargetType  AddChangeDeleteButtons �� Triggers��%�  &	        &	 (    7   �� 5   
   " ��5      .R�5      �� #��z��6   #��z��5      #��z��5      ._�5      �� #��z��6#   #��z��5      5       5      P� 5      	 � {�$
 False��5      . �5      �� # Z�5       � ���� 5       # Z
�5!       � ��5"      5$      �� 5%   
   " ��6   .�5&      ��  � �� �#fa477f6+   �� 5'       � ��
 �����6+   �� 5(       � ��$ 24(�6)   �� 5)       �@�Cursor$
 Hand��6'   �� 5*       � ��8   6)   �� 5+       � ��$ 0q�60    5,      P� 5-      	 �@J�IsMouseOver$

 True��61   . �5.      ��  � �� �#ff2f7163   5/      51      52      .��53      �� ���#01042d._�54   
   � .+�55      �� -   5        $��Right=�55      $	��Top=�62   $��	0,20,20,0q�6J   $��24��6]   
������ $��0q�56       � Cursor$
 Hand��6*   #��[  68   .��57      �� $��images\turnOff.png��6   58      &   System.Windows.Controls.DataGrid  59      -   5          Name$ dataGridCenter��59      $��Center=�6/   $��Center=�6J   $
��auto��6g   $
��auto��6t   ���#fff$��0q�6�      
IsReadOnly$
 True��5:         AlternatingRowBackground �#8345d8   RowBackground �#401aab   CanUserAddRows$ False��6]      SelectionUnit$ FullRow=�6t      SelectionMode$ Single=�6�      RowStyle 5;      �� 5<      )  #System.Windows.Controls.DataGridRow"  6   .�5=      ��  � �� �#401aab67   �� 5>       � �� �#fff67   �� 5?       � ��$ Normal"�67   5@         ColumnHeaderStyle 5B      �� 5C      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5D      ��  � �� �#fa477f67   �� 5E       � �� �#fff67   �� 5F       � ��$ 18(�65   5G      5I      �� 5J      $��Center=�6   $	��Top=�66   $
��auto��6N   $
��auto��6\   $��0,15,0,0q�6i    �� Orientation$ 
Horizontal=�6{    � UseLayoutRounding$ False��6�    5K      ��%  '��       7   �� 5L      " ��6   .�5M      ��  � ��$ 10,0,0,0q�63   �� 5N       �@�Cursor$
 Hand��63   �� 5O       � ��$ 0q�6<   �� 5P       � �� �#fff67   �� 5Q       � ��
 ���� 67   �� 5R       � ��8   65   5S      5T      .+�5U      �� -   5         ��Name$ btnProducts��5U      $��Products��6.   $
��Left=�6A   $	��Top=�6\   $��30��6t   $��80��6�   6�   �� 5V      -   5        $ btnCustomer��5V      $��Customer��6.   $��Bottom=�6A   $��30��6\   $��Right=�6h   $��80��6�   6�   �� 5W      -   5        $ 	btnBrands��5W      $��Brands��6,   $��Bottom=�6=   $��30��6X   $��Right=�6d   $��80��6�   6�   �� 5X      -   5        $ 	btnModels��5X      $��Models��6,   $��Bottom=�6=   $��30��6X   $��Right=�6d   $��80��6�   6�   �� 5Y      -   5        $ btnType��5Y      $��Product types��6*   $��Bottom=�6B   $��30��6]   $��Right=�6i   $��80��6�   6�   �� 5Z      -	   5        $ 
btnPayment��5Z      $��Payment��6-   $��Bottom=�6?   $��30��6Z   $��Right=�6f   $��80��6�   6�   �� 5[      -
   5        $ 	btnServis��5[      $��Servis��6,   $��Bottom=�6=   $��30��6X   $��Right=�6d   $��80��6�   6�   �� 5\      -   5        $ btnWarranty��5\      $��Warranty��6.   $��Bottom=�6A   $��30��6\   $��Right=�6h   $��80��6�   6�   5]      �� 5^      -   5        $ btnAdd��5^      $	��Add��6%   $��Center=�63   $��Bottom=�6P   $��-250,0,0,10q�6k   $��35��6�   $	��100��6�   $��0q�6�   $��24(�5_      $
 Hand��6#   #��[  61   #��[ 6g    5`      ��%_   '��       �� 5a      " ��6   .�5b      ��  �@��CornerRadius$ 10��69   5c      5d      5e      �� 5f      -   5        $ 	btnChange��5f      $��Change��6(   $��Bottom=�69   $��35��6T   $��Center=�6`   $��0,0,0,10q�6}   $	��100��6�   $��0q�5g      $��24(�6)   $
 Hand��67   #��[  6E   #��[ 6{    5h      ��%_   '��       �� 5i      " ��6   .�5j      ��  �@��CornerRadius$ 10��69   5k      5l      5m      �� 5n      -   5        $ 	btnDelete��5n      $��Delete��6(   $��Bottom=�69   $��35��6T   $��Center=�6`   $��
250,0,0,10q�6}   $	��100��6�   $��0q�6�   $
 Hand��5o      #��[  6#   #��[ 6Z    5p      ��%_   '��       �� 5q      " ��6   .�5r      ��  �@��CornerRadius$ 10��69   5s      5t      5u      5v      5w      5x      