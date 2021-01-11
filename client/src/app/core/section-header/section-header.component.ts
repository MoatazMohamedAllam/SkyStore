import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {

  beadcrumbs$: Observable<any[]>;

  constructor(private bcService:BreadcrumbService) { }

  ngOnInit(): void {
    this.beadcrumbs$ = this.bcService.breadcrumbs$
  }

}
