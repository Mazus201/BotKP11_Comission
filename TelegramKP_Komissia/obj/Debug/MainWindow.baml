   M S B A M L   `   `   `  ����N  JTelegramKP_Komissia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TelegramKP_KomissiaTelegramKP_Komissia  5   	   $    TelegramKP_Komissia.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:TelegramKP_Komissia     5   	     � WindowStartupLocation$  CenterScreen=�5       � WindowStyle$ ThreeDBorderWindow=�5   	    � Title$ Support TeleBot��5	   	   $	��800��6!   $	��800��6.   $	��600��5
   !   $	��400��61   .��5      � ���	#FFE4DBBF � RowDefinitions 5   
   �� 5      �� 5      $	��160 �6    � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      .+�5   
   �� $��0��6   $��0��5      $��2��5         BorderStyle#��[  5      ._�5      �� -   5        $��LBUsers��5      $
��18pt(�5      ��5      �� 5      .R�5      �� ��6(   �� *Nick+5      5      5       �� 5!      $��Bottom=�6   $��1��5"      $��12q�5#      $��Center=�5$      �� 5%      -   5        .�5%   8   Статистика5'      5(      �� 5)   
   $��1��6   $��0��5*      #��[  5+      ._�5,      �� -   5        $��
LBMessateg��5,      $	��200��5-      $
��16pt(�5.      $
��Left=�5/      ��50      ��  �� ElementName$ LBUsers�� �� Path$ SelectedItem.Messages�51      52      �� 53   
   $��1��6   $��1��54      #��[  55      ._�56      �� .+�57      �� -   5        $��
TxbMessage��58      $��90��57      $
��18pt(�59      5:      �� 5;      -   5        $��BtnSend��5>      $��Отправить��5;      $
��auto��5<      $
��0, 5q�5=      $��22(�5?      ���#DC5B21
�������$
��Bold"�5C      5D      5E      5F      5G      