   M S B A M L   `   `   `  ����K  GApplication-CLOs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Application_CLOsApplication_CLOs  5   	   (    "Application_CLOs.ViewStudentResult   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Application_CLOs     5   	   -   5          � Title$  ViewStudentResult��5   	   $	��450��6#   $	��800��60   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��49* �6   �� 5      $
��415* �6    � ColumnDefinitions 5   
   �� 5      .+�5   
   � $��0��6    5      �� 5      $	��99* �6#   �� 5      $	��36* �6#   �� 5      $	��25* �6#   .+�5      �� -   5         ��Name$ btnClose��5      $��2��6'   $��Close��67   $��	20,10,0,0q�6G   $��24��6Z   $��90��6f   ���	#FFC82530
�������$
��Left=�6�   $	��Top=�6�   6�   �� 5      -   5         ��Name$ txtbxSearch��5       �� TextWrapping$
 Wrap=�6+   $��Search Here��6?   $	��624��6R    �� TextAlignment$ Center=�6^   $��Center=�6u   $��2��6�   $��Center=�6�   $��18��6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	dgStudent��5   *      CanUserAddRows .  $��0,44,0,0q�6h   $��2��6z   	   Columns	 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Edit��6)      CellTemplate 5      �� 5      .R�5       �� $��btnEdit��6%   -   5        $
��Edit��5    4   6C   5!      5#        5$      $
 Delete��6)    5%      �� 5&      .R�5'      �� $��	btnDelete��6%   -   5        $��Delete��5'   6   6G   5(      5*      5,      5.      5/      