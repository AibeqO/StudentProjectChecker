   M S B A M L   `   `   `  ����Q  MStudentsProjectChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:StudentsProjectCheckerStudentsProjectChecker  5   	   *    $StudentsProjectChecker.TeacherWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:StudentsProjectChecker     5   	     � Title$  TeacherWindow��5   	   $	��450��6   $	��800��6,   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ projectDataGrid��5
      $��0,44,0,0q�6,      
IsReadOnly$
 True��6>      AutoGenerateColumns$
 True��6P   �� 5   
   -   5         ��Name$ 	finButton��5      $��
Поиск��6$   $
��Left=�64   $��	20,10,0,0q�6O   $	��Top=�6b   $	��144��6z   $��30��6�   6�   	� 5   
   -   5         	�Name$ findByGroupName��5      $$��Поиск по группам��6/   $
��Left=�6J   $��
197,18,0,0q�6e   $	��Top=�6y   6�   	� 5   
   -   5        $ findByStudentName��5      $$��Поиск по Фамилии��61   $��Center=�6L   $��0,18,0,0q�6i   $	��Top=�6{   �� 5   
   -   5         ��Name$ dataForFind��5      $
��Left=�6'   $��
610,14,0,0q�6B   $�� ��6V    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $	��169��6�   $��Times New Roman)�6�   $��18(�6�   �� 5   
   $&�� Данные для поиска��6   $
��Left=�6,   $��
488,13,0,0q�6G   $	��Top=�6[   5      5      