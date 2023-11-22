import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  public products:any =[];
  constructor(private auth:AuthService, private api:ApiService){}

  ngOnInit(): void {
      this.api.getProducts()
      .subscribe(res =>
        this.products = res
      );
  }


  logout(){
    this.auth.signOut();
  }
}
