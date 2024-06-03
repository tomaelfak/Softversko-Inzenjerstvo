import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {AuthService} from "../../../auth/services/auth.service";
import {MatDialog} from "@angular/material/dialog";
import {EditBioDialogComponent} from "../edit-bio-dialog/edit-bio-dialog.component";

@Component({
  selector: 'app-player-page',
  templateUrl: './player-page.component.html',
  styleUrl: './player-page.component.css'
})
export class PlayerPageComponent implements OnInit, OnDestroy{
  username: string = "";
  belongsToCurrentUser: boolean = false;
  bio = "This is a field for a players biography. I am young guy who loves to play football and tennis, but\n" +
    "            can find out With basketball also. Let's have some competitive games together!";
  constructor(private route: ActivatedRoute,
              private authService: AuthService,
              private bioDialog: MatDialog) {
  }

  ngOnInit(): void {
    this.username = this.route.snapshot.params['username'];
    this.belongsToCurrentUser = this.authService.user.value?.username === this.username;

  }

  ngOnDestroy(): void {
  }


  addImage(){

  }

  onEditBio(){
    const dialogRef = this.bioDialog.open(EditBioDialogComponent, {
      data: {bio: this.bio}
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.bio = result;
      }
    });
  }

}