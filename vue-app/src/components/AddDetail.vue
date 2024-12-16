<template>
  <el-dialog v-model="dialogVisible" :title="dialogTitle" width="35%" :before-close="handleClose">
    <el-form :model="ruleFroms" label-width="auto" style="max-width: 600px">
      <!-- 动态渲染表单项 -->
      <template v-if="ruleFroms.name === '供应商档案'">
        <el-form-item label="名称" prop="fieldName">
          <el-input v-model="ruleFroms.fieldName" />
        </el-form-item>
        <el-form-item label="供应商地址" prop="supplier_address">
          <el-input v-model="ruleFroms.supplier_address" />
        </el-form-item>
        <el-form-item label="供应商电话" prop="supplier_phone">
          <el-input v-model="ruleFroms.supplier_phone" />
        </el-form-item>
      </template>
      <template v-else-if="ruleFroms.name === '客户档案'">
        <el-form-item label="客户姓名" prop="fieldName">
          <el-input v-model="ruleFroms.fieldName" />
        </el-form-item>
        <el-form-item label="客户地址" prop="customer_address">
          <el-input v-model="ruleFroms.customer_address" />
        </el-form-item>
        <el-form-item label="客户电话" prop="customer_phone_number">
          <el-input v-model="ruleFroms.customer_phone_number" />
        </el-form-item>
      </template>
      <template v-else-if="ruleFroms.name === '商品档案'">
        <el-form-item label="商品编码" prop="product_code">
          <el-input v-model="ruleFroms.product_code" />
        </el-form-item>
        <el-form-item label="商品类型" prop="product_type">
          <el-input v-model="ruleFroms.product_type" />
        </el-form-item>
        <el-form-item label="商品价格" prop="unit_price">
          <el-input v-model="ruleFroms.unit_price" />
        </el-form-item>
      </template>
      <template v-else-if="ruleFroms.name === '仓库档案'">
        <el-form-item label="存货名称" prop="fieldName">
          <el-input v-model="ruleFroms.fieldName" />
        </el-form-item>
        <el-form-item label="存货编码" prop="product_code">
          <el-input v-model="ruleFroms.product_code" />
        </el-form-item>
        <el-form-item label="数量" prop="count">
          <el-input v-model="ruleFroms.count" />
        </el-form-item>
        <el-form-item label="价单" prop="unit_price">
          <el-input v-model="ruleFroms.unit_price" />
        </el-form-item>
        <el-form-item label="总单" prop="total_order">
          <el-input v-model="ruleFroms.total_order" />
        </el-form-item>
      </template>
      <template v-else>
        <el-form-item label="名称" prop="fieldName">
          <el-input v-model="ruleFroms.fieldName" />
        </el-form-item>
        <el-form-item label="值" prop="fieldValue">
          <el-input v-model="ruleFroms.fieldValue" />
        </el-form-item>
      </template>
    </el-form>

    <!-- 底部按钮 -->
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
  tableRow: { type: Object },
  id: {
    type: String,
    required: true
  }
});

const realod = inject("getList");

const state = reactive({
  dialogVisible: false,
  ruleFroms: {
    id: "",
    name: "",
    fieldName: "",
    fieldValue: "", 
    supplier_address: "",
    supplier_phone: "",
    customer_address: "",
    customer_phone_number: "",
    product_code: "",
    product_type: "",
    unit_price: "",
    count: "",
    total_order: "",
  },
});

  //监听器
  watch(
    () => props.tableRow,
    () => {
      state.ruleFroms = { ...props.tableRow }; // 自动填充 fieldName 和 fieldValue
    },
    { deep: true, immediate: true }
  );
  
  const dialogCategory = () => {
    state.dialogVisible = true;
  };
  
  // 提交修改
  const addCategory = () => {
  // 构造动态参数对象，只包含非空字段
  const param2 = {
    categoryId: props.id, // 父组件传递的分类ID
  };

  for (const [key, value] of Object.entries(state.ruleFroms)) {
    if (value !== null && value !== "" && key !== "id" && key !== "name") {
      param2[key] = value;
    }
  }

  if (!props.id) {
    ElMessage.error("缺少有效的分类 ID");
    return; // 提前返回，不继续执行
  }

  if (props.dialogTitle === "添加") {
    // 添加操作
    if (!param2.categoryId) {
      ElMessage.error("缺少分类 ID");
      return;
    }
    axios.post(`/CategoryData/${param2.categoryId}`, param2)
      .then(() => {
        ElMessage.success("添加成功");
        state.dialogVisible = false; // 关闭窗口
        realod(); // 刷新列表
      })
      .catch((error) => {
        ElMessage.error("添加失败: " + error.response.data.message);
      });
  } else {
    // 修改操作
    if (!props.tableRow.id) {
      ElMessage.error("缺少有效的 ID");
      return;
    }
    axios.put(`/CategoryData/${props.tableRow.id}`, param2)
      .then(() => {
        ElMessage.success("修改成功");
        state.dialogVisible = false; // 关闭窗口
        realod(); // 刷新列表
      })
      .catch((error) => {
        ElMessage.error("修改失败: " + error.response.data.message);
      });
  }
};



// 解构
const { dialogVisible, ruleFroms } = toRefs(state);

// 暴露给父组件的方法
defineExpose({ dialogCategory });
</script>
