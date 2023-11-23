import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ApiService } from '../../services/api.service';
import { UserStoreService } from '../../services/user-store.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NgToastModule, NgToastService } from 'ng-angular-popup';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,NgToastModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  public products:any =[];
  fullName!: string;

  addItemForm: FormGroup;
  editItemForm: FormGroup;

  // Sample API URL, replace it with your actual API endpoint
  apiUrl = 'https://your-api-endpoint.com/items';

  // Sample data for the category dropdown, replace it with your actual data
  categories :any =[];
  modalService: any;

  constructor(
    private auth:AuthService,
    private api:ApiService,
    private userStore: UserStoreService,
    private fb: FormBuilder,
    private toast: NgToastService
    ){

    this.addItemForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      amount: [null, [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      isActive: [false]
    });

    this.editItemForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      amount: [null, [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      isActive: [false]
    });
  }

  ngOnInit(): void {

    this.buildForm();

    this.userStore.getFullName()
    .subscribe(res=>{
      let value = this.auth.getFullNameFromToken();
      this.fullName = res || value;
    })


      this.api.getProducts()
      .subscribe(res =>
        this.products = res
      );

      this.api.getCategories()
      .subscribe(res =>
        this.categories = res
      );

  }


  logout(){
    this.auth.signOut();
  }

  buildForm() {
    this.addItemForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      amount: [null, [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      isActive: [false]
    });
  }
  onSubmit() {
    if (this.addItemForm.valid) {
      const product = this.addItemForm.value;

      this.api.addProduct(product)
      .subscribe({
        next: (res) => {
          this.addItemForm.reset();

          this.toast.success({detail:"Success",summary:'Add Product with Success',duration:5000});

          this.refreshProductList();
        },
        error:(err) => {
         console.log(err?.error.message);
         this.toast.error({detail:"ERROR",summary:err.error.message,duration:5000});
        }
      });
    }
  }

  onDelete(product: any) {
    if(confirm("Are you sure to delete "+product.name)) {
      this.api.deleteProduct(product.id)
      .subscribe({
        next: (res) => {
          this.toast.success({detail:"Success",summary:'Delete Product with Success',duration:5000});

          this.refreshProductList();
        },
        error:(err) => {
         console.log(err?.error.message);
         this.toast.error({detail:"ERROR",summary:err.error.message,duration:5000});
        }
      });
    }
  }

  refreshProductList() {
    // Refresh the list of products by making an API call or any other method
    this.api.getProducts()
      .subscribe(res => {
        this.products = res;
      });
  }
}
