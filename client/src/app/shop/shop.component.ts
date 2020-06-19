import { Component, OnInit } from '@angular/core';
import {IProduct} from '../shared/models/product';
import {ShopService} from './shop.service';
import {IBrand} from '../shared/models/brand';
import {IType} from '../shared/models/productType';
import {ShopParams} from '../shared/models/shopParams';
import {tap} from 'rxjs/operators';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  totalCount: number;
  shopParams = new ShopParams();
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];


  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts = () => {
    let { pageNumber, pageSize } = this.shopParams;
    this.shopService.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      pageNumber = response.pageIndex;
      pageSize = response.pageSize;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  getBrands = () => {
    this.shopService.getBrands().pipe(tap(res => console.log(res))).subscribe(response => {
      this.brands = [{id: 0, name: 'All'}, ...response];
      }, error => console.log(error));
  }

  getTypes = () => {
    this.shopService.getTypes().pipe(tap(res => console.log(res))).subscribe(response => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => console.log(error));
  }

  onBrandSelected = (brandId: number) => {
    this.shopParams.brandId = brandId;
    this.getProducts();
}

onTypeSelected = (typeId: number) => {
    this.shopParams.typeId = typeId;
    this.getProducts();
}

onSortSelected = (sort: string) => {
    this.shopParams.sort = sort;
    this.getProducts();
}

onPageChanged = (event: any) => {
    this.shopParams.pageNumber = event;
    this.getProducts();
}
}
