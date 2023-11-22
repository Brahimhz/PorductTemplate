import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { UserStoreService } from '../../services/user-store.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  public products:any =[];
  fullName!: string;

  constructor(private auth:AuthService, private api:ApiService, private userStore: UserStoreService){}

  ngOnInit(): void {

    this.userStore.getFullName()
    .subscribe(res=>{
      let value = this.auth.getFullNameFromToken();
      this.fullName = res || value;
    })


      this.api.getProducts()
      .subscribe(res =>
        this.products = res
      );
  }


  logout(){
    this.auth.signOut();
  }
}
