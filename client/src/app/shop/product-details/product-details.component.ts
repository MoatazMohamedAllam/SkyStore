import { IProduct } from './../../shared/models/product';
import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopService:ShopService, 
              private activateRouter:ActivatedRoute,
              private bcService:BreadcrumbService) 
              {
                this.bcService.set('@productDetails','');
               }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    var id = this.activateRouter.snapshot.paramMap.get('id');
    this.shopService.getProduct(+id)
      .subscribe(product =>{
          this.product = product;
          this.bcService.set('@productDetails',product.name);
        }, err => console.log(err));
  }

}
