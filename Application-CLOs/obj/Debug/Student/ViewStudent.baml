   M S B A M L   `   `   `  ����K  GApplication-CLOs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Application_CLOsApplication_CLOs  5   	   "    Application_CLOs.ViewStudent   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Application_CLOs     5   	   -   5          � Title$  ViewStudents��5   	   $	��480��6   $	��800��6+    � WindowStartupLocation$ CenterScreen=�67    � 	Resources 5	       �� 
TargetType �� 
ResourceId �� TypeInTargetAssembly&   System.Windows.Controls.DataGrid �� Property �� Value �� 
TargetType	 ��Name
 J� SnapsToDevicePixels �Name J� Opacity � Points �� Stretch �� Triggers P� Property P� Value �� 
TargetName   DataGridStyle1 �� Padding ��Name � ColumnDefinitions � RowDefinitions   SelectAllCommand7  1System.Windows.Controls.DataGridHeadersVisibility
   All �� ConverterParameter!   HeadersVisibilityConverter �� 	Converter �� RelativeSource �� AncestorTypeG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter  Name   Column  ��Name! �� CanContentScroll" ��Name# �� ScrollableHeight$ � Maximum% �� Orientation& �� ViewportHeight' �� ViewportSize*( �� #ComputedVerticalScrollBarVisibility) � Value* �� Mode+ �� ScrollableWidth, �� ViewportWidth,- �� %ComputedHorizontalScrollBarVisibility. �� Triggers/ ]� 
Conditions0 �� Property1 �� Value��%>  (��       5
      $" DataGridSelectAllButtonStyle��"  )&	  �    �� 6
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $	 Border��6(   #��� ��68   
 . 6y   � 5      $ Arrow��6&   
���   �$��Right=�6B   $��8,8,3,3q�6^   $
 0.15Q�6o    �   ��� ��� ��� ��� $ Uniform=�6�   $��Bottom=�6�   5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ Border��6;   # � ��6O    � ��5      P� 5       � �� . . �5   "   �� $ Border��69   # � ��6M    � ��5      P� 5       � {� .  . �5   "   �� $ Arrow��6?    � y�$ 	Collapsed=�6R   5      5      5      5       �� 5!   
   "  6'   .�5"      ��  � ��# � ��6+   �� 5#       � ��# � ��6+   �� 5$       � �� �	#FF688CAF6,   �� 5%       � ��$ 1q�60   �� 5&        �@ RowDetailsVisibilityMode$ VisibleWhenSelected=�69   �� 5'       � �� . 6>   �� 5(       �@��PanningMode$
 Both=�69   �� 5)       �@��IsFlicksEnabled .  67   �� 5*       � �� 5+      �� 5,      "  6&   .R�5-      �� #��z��6!   #��z��6K   #��z��6w   # z��6�   
 . ._�5.      �� $ DG_ScrollViewer��6+   |�.  ��5/   "   �� 50   &   " ��66   .R�51   *   �  52   .   �� 53   2   $
��Auto �6C   �� 54   2   $��* �6C   �� 55   2   $
��Auto �6C    57   .   �� 58   2   $
��Auto �6@   �� 59   2   $��* �6@   �� 5:   2   $
��Auto �6@   .+�5<   .   �� #��Z 65   |�.  ��6v   C� *�� $" DataGridSelectAllButtonStyle��"  +y�6�   �� *HeadersVisibility+# Z # Z  �� "  ��6�  �� *CellsPanelHorizontalOffset+ �� "    5=   .   $! PART_ColumnHeadersPresenter��6M   $��1��6r   y�6�   �� *HeadersVisibility+# Z # Z  �� "  �� 5>   .   $!  PART_ScrollContentPresenter��6E   #! z��6j   $��2��6�   $��1��6�   �� 5?   .   $" PART_VerticalScrollBar��68   $��2��6X   #$ z# 6h   $% Vertical=�6�   $��1��6�   #' z& 6  #y�z( 6H  ) 6�   �� *VerticalOffset+$* OneWay=� �� *TemplatedParent+6H  � 5@   .   $��1��63   $��2��6C    5A   2   �� 5B   6   ��6G   �� **(NonFrozenColumnsViewportHorizontalOffset+ �� "  �� 5C   6   $��* �6G   .+�5E   2   �� $" PART_HorizontalScrollBar��6<   $��1��6^   #$ z+ 6n   $% 
Horizontal=�6�   #' z, 6  #y�z- 6C  ) 6�   �� *HorizontalOffset+$* OneWay=� �� *TemplatedParent+6C  5F   /   5G   +   5H   '   .��5J   "   �� #
 z
 61   5K      5L      5M      5O      . 5P      ]� 5Q      / 5R      �� 5S      0 �@��
IsGrouping1 . 6:   �� 5T      "0 �@%�IsVirtualizingWhenGrouping1 .  6\   ./�5V      ��  � �� .  6F   5W      5Y      5Z      .��5[      �  5\   
   �� 5]      $	��49* �6   �� 5^      $
��415* �6    5`   
   �� 5a      .+�5c   
   � $��0��6    5d      �� 5e      $	��99* �6#   �� 5f      $	��36* �6#   �� 5g      $	��25* �6#   .+�5i      �� -   5        2 ��Name$2 btnClose��5i      $��2��6'   $��Close��67   $��	20,10,0,0q�6G   $��24��6Z   $��90��6f   ���	#FFC82530
�������$
��Left=�6�   $	��Top=�6�   6�   �� 5j      -   5        3 ��Name$3 txtbxSearch��5j      4 �� TextWrapping$
4 Wrap=�6+   $��Search Here��6?   $	��624��6R   5 �� TextAlignment$5 Center=�6^   $��Center=�6u   $��2��6�   $��Center=�6�   $��18��6�   5l        5m   
   -   5        6  Name$6 	dgStudent��5m   *   7   CanUserAddRows7 .  $��0,44,0,0q�6h   $��2��6z   8   Columns8 5n      4  .System.Windows.Controls.DataGridTemplateColumn  5o      ,  &System.Windows.Controls.DataGridColumn9   Header$
9 Edit��6)   :   CellTemplate: 5p      �� 5q      .R�5r      �� $��btnEdit��6%   -   5        $
��Edit��5r   4   6C   5s      5u        5v      $9 Delete��6)   : 5w      �� 5x      .R�5y      �� $2 	btnDelete��6%   -   5        $��Delete��5y   8   6I   5z      5|      5~      5�      5�      