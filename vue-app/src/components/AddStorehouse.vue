<template>
    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="35%" :before-close="handleClose">
      <el-form :model="ruleFroms" label-width="auto" style="max-width: 600px">
        <el-form-item label="仓库名称" prop="name">
          <el-input v-model="ruleFroms.name" />
        </el-form-item>
        <el-form-item label="仓库地址" prop="storehouse_address">
          <el-input v-model="ruleFroms.storehouse_address" />
        </el-form-item>
        <el-form-item label="商品编码" prop="product_code">
          <el-input v-model="ruleFroms.product_code" />
        </el-form-item>
        <el-form-item label="商品类型" prop="product_type">
          <el-input v-model="ruleFroms.product_type" />
        </el-form-item>
        <el-form-item label="单价" prop="unit_price">
          <el-input v-model="ruleFroms.unit_price" />
        </el-form-item>
        <el-form-item label="数量" prop="count">
          <el-input v-model="ruleFroms.count" />
        </el-form-item>
      </el-form>
  





      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="addCategory">提交</el-button>
        </div>
      </template>
    </el-dialog>
  </template>
  



  
  <script setup>
  import axios from '@/api/api_config';
  import { reactive, toRefs, watch, inject } from 'vue';
  import { ElMessage } from 'element-plus';
  
  // 定义 props
  const props = defineProps({
    dialogTitle: { type: String },
    tableRow: { type: Object }
  });
  
  const realod = inject("getList")
  
  const state = reactive({
    dialogVisible: false,
    ruleFroms: {
      id: "",
      name: "",
      storehouse_address: "", // 新增描述字段
      product_code: "", 
      product_type: "", 
      unit_price: "", 
      count: "", 
    },
  });
  

  // 提交修改
  const addCategory = () => {
      const param = {
          name: ruleFroms.value.name,
          storehouse_address: ruleFroms.value.storehouse_address, 
          product_code: ruleFroms.value.product_code, 
          product_type: ruleFroms.value.product_type, 
          unit_price: ruleFroms.value.unit_price, 
          count: ruleFroms.value.count, 
          description: ruleFroms.value.description, 
      };
  
      if (props.dialogTitle === "添加") {
          axios.post("/File_Management/appStorehouse_Table", param).then(() => {
              ElMessage.success("添加成功");
              state.dialogVisible = false; // 关闭窗口
              realod(); // 刷新列表
          });
      } else {
          axios.put(`/File_Management/storehouseid/${props.tableRow.id}`, param).then(() => {
              ElMessage.success("修改成功");
              state.dialogVisible = false; // 关闭窗口
              realod(); // 刷新列表
          });
      }
  };
  
  //监听器
  watch(
      () => props.tableRow,
      () => {
          state.ruleFroms = { ...props.tableRow }; // 自动填充 name 和 description
      },
      { deep: true, immediate: true }
  );
  
  const dialogCategory = () => {
    state.dialogVisible = true;
  
  
  };
  
  
  
  
  //结构
  const { dialogVisible, ruleFroms } = toRefs(state);
  
  // 暴露给父组件的方法
  defineExpose({ dialogCategory });
  </script>