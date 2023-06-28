import { Component, TemplateRef, OnInit } from '@angular/core';
import { SummaryTagDto, TagsClient } from '../web-api-client';

@Component({
  selector: 'app-tags-component',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent implements OnInit {
  tags: SummaryTagDto[] = [];
  searchText:string = "";
  
  constructor(
    private tagClient:TagsClient
  ){}

  ngOnInit(): void {
    this.tagClient.summary().subscribe(
      (result) => {
        this.tags = result.sort((a, b) => a.count < b.count ? 1 : -1);
      }
    )
  }
}