import { Component, TemplateRef, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateTagCommand, TagDto, TagsClient, TodoListDto } from 'src/app/web-api-client';


@Component({
  selector: 'app-tag-filter-component',
  templateUrl: './tag-filter.component.html',
  styleUrls: ['./tag-filter.component.scss']
})
export class TagFilterComponent implements OnInit, OnChanges {
  @Input() selectedList: TodoListDto;
  tags:string[];
  @Input() selectedTags:string[];
  @Output() selectedTagsChange = new EventEmitter<string[]>();

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    this.populateTags();
  }

  ngOnInit(): void {
    this.populateTags();
  }

  populateTags() {
    let itemTags = this.selectedList.items.map(
      (item) => {
        return item.tags.map(tag => tag.name);
      }
    );
    const tags = [].concat.apply([], itemTags);
    this.tags = tags.filter((v, i, arr) => arr.indexOf(v) === i)
                    .sort();
  }

  onSelectTagFilter(tag:string) {
    const index = this.selectedTags.findIndex(t => t === tag);
    if (index > -1) 
      this.selectedTags.splice(index, 1);
    else
      this.selectedTags.push(tag);
  }

  isTagSelected(tag: string) {
    return this.selectedTags.findIndex(t => t === tag) > -1;
  }
}