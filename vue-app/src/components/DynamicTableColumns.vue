<template>
    <div>
      <!-- 动态列渲染 -->
      <template v-if="name === '供应商档案'">
        <el-table-column prop="fieldName" label="供应商名称" width="180" />
        <el-table-column prop="supplier_phone" label="供应商电话" width="200" />
        <el-table-column prop="supplier_address" label="供应商地址" width="200" />
      </template>
  
      <template v-if="name === '客户档案'">
        <el-table-column prop="fieldName" label="客户姓名" width="180" />
        <el-table-column prop="customer_address" label="客户地址" width="200" />
        <el-table-column prop="customer_phone_number" label="客户电话" width="200" />
      </template>
  
      <template v-if="name === '商品档案'">
        <el-table-column prop="product_code" label="商品编码" width="200" />
        <el-table-column prop="product_type" label="商品类型" width="200" />
        <el-table-column prop="unit_price" label="商品价格" width="200" />
      </template>
  
      <template v-if="name === '仓库档案'">
        <el-table-column prop="fieldName" label="存货名称" width="200" />
        <el-table-column prop="product_code" label="存货编码" width="200" />
        <el-table-column prop="count" label="数量" width="200" />
        <el-table-column prop="unit_price" label="价单" width="200" />
        <el-table-column prop="total_order" label="总单" width="200" />
      </template>
    </div>
  </template>
  
  <script>
//   import { defineProps } from 'vue';
  
  export default {
    props: {
      name: {
        type: String,
        required: true
      }
    }
  };
  </script>
  