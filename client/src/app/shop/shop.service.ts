import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {IPagination} from '../shared/models/pagination';
import {IBrand} from '../shared/models/brand';
import {IType} from '../shared/models/productType';
import {map} from 'rxjs/operators';
import {ShopParams} from '../shared/models/shopParams';
import {IProduct} from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {
  }


  getProducts = (shopParams: ShopParams) => {
    // So that I don't have to change anything beyond adding shopParams.
    const {brandId, typeId, sort, search} = shopParams;

    let params = new HttpParams();

    // Doesn't seem to work right unless it's greater than 0.
    if (brandId > 0){
      params = params.append('brandId', brandId.toString());
    }

    if (typeId !== 0){
      params = params.append('typeId', typeId.toString());
    }

    if (search){
      params = params.append('search', search);
    }

    params = params.append('sort', sort);

    params = params.append('pageIndex', shopParams.pageNumber.toString());

    params = params.append('pageIndex', shopParams.pageSize.toString());

    return this.http.get<IPagination>(`${this.baseUrl}products`, {observe: 'response', params})
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  getProduct = (id: number) =>  this.http.get<IProduct>(`${this.baseUrl}products/${id}`);

  getBrands = () => this.http.get<IBrand[]>(`${this.baseUrl}products/brands`);

  getTypes = () => this.http.get<IType[]>(`${this.baseUrl}products/types`);

}
