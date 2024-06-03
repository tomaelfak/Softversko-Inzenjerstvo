import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {AdminPanelService} from "../../services/admin-panel/admin-panel.service";
import {Subscription} from "rxjs";
import {UserInfo} from "../../interfaces/userInfo";
import {MatTable, MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent implements OnInit, OnDestroy, AfterViewInit{
  users: UserInfo[] = [];
  displayedColumns: string[] = ['username', 'role', 'toAdmin', 'toManager', 'toPlayer', 'delete'];
  @ViewChild(MatPaginator, {static: true}) paginator!: MatPaginator;
  @ViewChild(MatTable, {static: true}) table!: MatTable<any>;

  dataSource = new MatTableDataSource<UserInfo>(this.users);

  filterValue: string = '';


  usersListSubscription?: Subscription;

  constructor(private adminPanelService: AdminPanelService) {}

  ngOnInit() {
    this.adminPanelService.usersChanged.subscribe({
      next: (users) => {
        this.users = users;
        this.dataSource.data = this.users;
        //this.table.renderRows();
      },
      error: (error) => {
        console.log(error);
      }
    })

    this.adminPanelService.getUsers();
  }

  ngOnDestroy() {
    this.usersListSubscription?.unsubscribe();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator!;
  }

  onUpgradeToAdmin(username: string) {
    this.adminPanelService.upgradeToAdmin(username);
  }
  onUpgradeToManager(username: string) {
    this.adminPanelService.upgradeToManager(username);
  }

  onUpgradeToPlayer(username: string) {
    this.adminPanelService.upgradeToPlayer(username);
  }

  onDelete(username: string) {
    if(confirm('Are you sure you want to delete this user?')){
      this.adminPanelService.deleteUser(username);
    }
  }

  applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

}
