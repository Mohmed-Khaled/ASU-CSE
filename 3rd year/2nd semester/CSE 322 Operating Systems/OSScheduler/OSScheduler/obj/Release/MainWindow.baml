   M S B A M L   `   `   `  ����   OSScheduler    OSScheduler.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #��[B�  � WindowStartupLocation$  CenterScreen=�.���  � ColumnDefinitions �� $
��Auto ��� $��* � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��Number of processes: ���� -   $��NumberOfProcesses��$��0��$��1���� $��1��$��0��$��10q�$��2��.+��� $��Choose scheduler type:��	� -    	� 	GroupName$	 alg��
 � Tag$
 FCFS��$
��FCFS��	� -   $	 alg��$ SJFNP��$��SJF (Non Preemptive)��	� -   $	 alg��$
 SJFP��$��SJF (Premmptive)��	� -   $	 alg��$	 PNP��$��Priority (Non Preemptive)��	� -   $	 alg��$ PP��$��Priority (Premmptive)��	� -   $	 alg��$ RR��$��Round Robin���� -   $��2��$��1��.��Next