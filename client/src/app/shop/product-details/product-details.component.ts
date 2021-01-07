import { IProduct } from './../../shared/models/product';
import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopService:ShopService, 
              private activateRouter:ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    var id = this.activateRouter.snapshot.paramMap.get('id');
    this.shopService.getProduct(+id)
      .subscribe(product =>{
          this.product = product;
        }, err => console.log(err));
  }

}
